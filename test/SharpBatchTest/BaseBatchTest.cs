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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Abstraction;
using Xunit;

namespace SharpBatchTest
{
    public class BaseBatchTest
    {
        [Fact]
        public void BaseBatch_ToFileContentAndFileName()
        {
            //Arrange
            var localBatch = new BaseBatch();
            localBatch.BatchContext = new ContextInvoker()
            {
                SessionId = Guid.NewGuid()
            };


            //Act
            string fullFileName = "";
            var ex = Record.Exception(() => fullFileName = localBatch.ToFile("Text to write Text to write Text to write Text to write Text to write ", "test.txt"));

            //Assert
            Assert.Null(ex);
            Assert.Equal("test.txt", fullFileName);
        }

        [Theory]
        [InlineData("Test.txt", null, null, false, false)]
        [InlineData("Test", "txt", null, false, false)]
        [InlineData("Test", "txt", "C:\\windows\\temp\\", false, false)]
        [InlineData("Test", "txt", "C:\\windows\\temp\\", false, true)]
        public void BaseBatch_ToFile(string fileName, string fileExtention, string path, bool timeStampToken, bool sessionIdInFileName)
        {
            //Arrange
            
            var localBatch = new BaseBatch();
            localBatch.BatchContext = new ContextInvoker()
            {
                SessionId = Guid.NewGuid()
            };
            var ExpectedFileName = createFullFileName(fileName, fileExtention, path, timeStampToken, sessionIdInFileName, localBatch.BatchContext );

            //Act
            string fullFileName = "";
            var ex = Record.Exception(() => fullFileName = localBatch.ToFile("Text to write Text to write Text to write Text to write Text to write ", 
                                                              fileName, 
                                                              fileExtention, 
                                                              path, 
                                                              timeStampToken, 
                                                              sessionIdInFileName));

            //Assert
            Assert.Null(ex);
            Assert.Equal(ExpectedFileName, fullFileName);
        }

        [Fact]
        public void BaseBatch_ToTrackingStringContent()
        {
            //Arrange
            var serializedContent = "";

            var modelSerializer = new Mock<IModelSerializer>(MockBehavior.Strict);
            modelSerializer.Setup((s) => s.Serialize(It.IsAny<object>())).Returns(serializedContent);

            var trackingProvider = new Mock<ISharpBatchTracking>(MockBehavior.Strict);
            trackingProvider.Setup((s) => s.AddMessageAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();

            var trackingFactory = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            trackingFactory.Setup((s) => s.getTrakingProvider()).Returns(trackingProvider.Object).Verifiable();

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);
            requestService.Setup((s) => s.GetService(It.Is<Type>(t => t.Name.Equals(nameof(ISharpBatchTrackingFactory)))))
                .Returns(trackingFactory.Object)
                .Verifiable();
            requestService.Setup((s) => s.GetService(It.Is<Type>(t => t.Name.Equals(nameof(IModelSerializer)))))
                .Returns(modelSerializer.Object)
                .Verifiable();

            Stream bodyStream = new MemoryStream();
            var httpResponse = new Mock<HttpResponse>(MockBehavior.Strict);
            httpResponse.Setup((s) => s.Body).Returns(bodyStream).Verifiable();

            var httpRequest = new Mock<HttpRequest>(MockBehavior.Strict);
            httpRequest.Setup((s) => s.QueryString)
               .Returns(new QueryString("?param=123"));

            var localBatch = new BaseBatch();

            var context = new Mock<HttpContext>(MockBehavior.Strict);
            context.Setup((s) => s.RequestServices).Returns(requestService.Object).Verifiable();
            context.Setup((s) => s.Request).Returns(httpRequest.Object).Verifiable();
            context.Setup((s) => s.Response).Returns(httpResponse.Object).Verifiable();


            localBatch.BatchContext = ContextInvoker.Create(context.Object);
            localBatch.BatchContext.SessionId = Guid.NewGuid();

            var dataTotrack = new TestData()
            {
                Name = "name",
                Surname = "surname"
            };

            //Act
            var ex = Record.Exception(() =>
            {
                localBatch.ToTracking(dataTotrack);
                requestService.Verify();
            });

            //Assert
            Assert.Null(ex);
        }
        private string createFullFileName(string fileName, string fileExtention, string path, bool timeStampToken, bool sessionIdInFileName, ContextInvoker context)
        {
            var fullFileName = $"{path ?? ""}{fileName}";
            if (sessionIdInFileName)
            {
                fullFileName += $"-{context.SessionId.ToString()}";
            }

            if (timeStampToken)
            {
                fullFileName += $"-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            }

            if (!string.IsNullOrEmpty(fileExtention))
            {
                fullFileName += $".{fileExtention}";
            }
            return fullFileName;
        }

        private class TestData
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }
    }
}
