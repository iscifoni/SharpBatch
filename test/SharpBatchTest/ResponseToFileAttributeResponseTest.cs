//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpBatch;
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest
{
    public class ResponseToFileAttributeResponseTest
    {
        [Theory]
        [InlineData("txt", "FILENAME",false, false, "", "FILENAME.txt", "ce8a738b-2534-4060-9f1c-9da6eba4aef8")]
        [InlineData("txt", "FILENAME", true, false, "", "FILENAME-ce8a738b-2534-4060-9f1c-9da6eba4aef8.txt", "ce8a738b-2534-4060-9f1c-9da6eba4aef8")]
        public void ResponseToFileAttributeResponse_FullFileName(string fileExtention, string fileName, 
                                                                bool sessionIdInFileName, bool timeStampToken, 
                                                                string path, string expected, string sessionId)
        {
            //Arrange
            var attribute = createAttribute(fileExtention, fileName, sessionIdInFileName, timeStampToken, path);
            var context = createBatchExecutionContext(new Guid(sessionId), "TEST TEST TEST");
            
            //Act
            attribute.onExecuted(context);

            //Assert
            Assert.Equal(expected, attribute.FullFileName);

        }

        [Fact]
        public void ResponseToFileAttributeResponse_onExecuted_StringContent()
        {
            //Arrange
            var attribute = createAttribute("txt", "FILENAME", false, false, "");
            var context = createBatchExecutionContext(Guid.NewGuid(), "TEST TEST TEST");

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            Assert.Null(ex);
        }

        [Fact]
        public void ResponseToFileAttributeResponse_onExecuted_ByteArrayContent()
        {
            //Arrange
            var attribute = createAttribute("txt", "FILENAME", false, false, "");
            var context = createBatchExecutionContext(Guid.NewGuid(), new byte[] { 123, 221, 213, 128, 165} );

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            Assert.Null(ex);
        }

        [Fact]
        public void ResponseToFileAttributeResponse_onExecuted_NotSavedClassContent()
        {
            //Arrange
            var attribute = createAttribute("txt", "FILENAME", false, false, "");
            var context = createBatchExecutionContext(Guid.NewGuid(), new WrongType());

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            Assert.NotNull(ex);
        }

        [Fact]
        public void ResponseToFileAttributeResponse_onExecuted_PathSpecified()
        {
            //Arrange
            var attribute = createAttribute("txt", "FILENAME", false, false, Path.GetTempPath());
            var context = createBatchExecutionContext(Guid.NewGuid(), new byte[] { 123, 221, 213, 128, 165 });

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            Assert.Null(ex);
        }
        private BatchExecutionContext createBatchExecutionContext(Guid sessionId, object content)
        {
            var shareMessage = new ShareMessageCollection();
            var responseObject = new ResponseObject(content, sessionId);
            shareMessage.Set<IResponseObject>(responseObject);

            BatchExecutionContext context = new BatchExecutionContext()
            {
                SessionId =sessionId,
                ShareMessage = shareMessage
            };

            return context;
        }

        private ResponseToFileAttribute createAttribute(string fileExtention, string fileName,
                                                                bool sessionIdInFileName, bool timeStampToken,
                                                                string path)
        {
            return new ResponseToFileAttribute()
            {
                FileExention = fileExtention,
                FileName = fileName,
                SessionIdInFileName = sessionIdInFileName,
                TimeStampTocken = timeStampToken,
                Path = path
            };
        }

        private class WrongType
        {
            public string Property1 { get; }
        }
    }
}
