﻿//Copyright 2016 Scifoni Ivano
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
using System.Text;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Abstraction;
using Xunit;
using Moq;


namespace SharpBatchTest
{
    public class ResponseToTrackingAttributeText
    {
        [Fact]
        public void ResponseToTrackingAttribute_SerializerStringTest()
        {
            //Arrange
            var sessionID = Guid.NewGuid();
            object content = "Test";
            var serializedContent = $"\"{content}\"";
            var attribute = new ResponseToTrackingAttribute();
            var context = createBatchExecutionContext(sessionID, content);

            var modelSerializer = new Mock<IModelSerializer>(MockBehavior.Strict);
            modelSerializer.Setup((s) => s.Serialize(content)).Returns(serializedContent).Verifiable();

            var trackingService = new Mock<ISharpBatchTracking>(MockBehavior.Strict);
            trackingService.Setup((s) => s.AddMessageAsync(sessionID, serializedContent)).Returns(Task.CompletedTask).Verifiable();

            var trackingFactoryService = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            trackingFactoryService.Setup((s) => s.getTrakingProvider()).Returns(trackingService.Object).Verifiable();

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);
            requestService.Setup((s) => s.GetService(It.Is<Type>(t => t.Name.Equals(nameof(ISharpBatchTrackingFactory))))).Returns(trackingFactoryService.Object).Verifiable();
            requestService.Setup((s) => s.GetService(It.Is<Type>(t => t.Name.Equals(nameof(IModelSerializer))))).Returns(modelSerializer.Object).Verifiable();

            context.RequestServices = requestService.Object;

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            requestService.Verify();
            Assert.Null(ex);
        }
        [Fact]
        public void ResponseToTrackingAttribute_SerializerClassTest()
        {
            //Arrange
            var sessionID = Guid.NewGuid();
            object content = new modelTest()
            {
                Name = "Name",
                Surname = "Surname"
            };
            var serializedContent = "{ \"Name\":\"Name\", \"Surname\":\"Surname\" }";

            var modelSerializer = new Mock<IModelSerializer>(MockBehavior.Strict);
            modelSerializer.Setup((s) => s.Serialize(It.IsAny<object>())).Returns(serializedContent);
            
            var attribute = new ResponseToTrackingAttribute();
            var context = createBatchExecutionContext(sessionID, content);

            
            var trackingService = new Mock<ISharpBatchTracking>(MockBehavior.Strict);
            trackingService.Setup((s) => s.AddMessageAsync(sessionID, serializedContent)).Returns(Task.CompletedTask).Verifiable();

            var trackingFactoryService = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            trackingFactoryService.Setup((s) => s.getTrakingProvider()).Returns(trackingService.Object).Verifiable();

            var requestService = new Mock<IServiceProvider>(MockBehavior.Strict);
            requestService.Setup((s) => s.GetService(It.Is<Type>(t=> t.Name.Equals(nameof(ISharpBatchTrackingFactory))))).Returns(trackingFactoryService.Object).Verifiable();
            requestService.Setup((s) => s.GetService(It.Is<Type>(t => t.Name.Equals(nameof(IModelSerializer))))).Returns(modelSerializer.Object).Verifiable();

            context.RequestServices = requestService.Object;

            //Act
            var ex = Record.Exception(() => attribute.onExecuted(context));

            //Assert
            requestService.Verify();
            Assert.Null(ex);
        }

        private BatchExecutionContext createBatchExecutionContext(Guid sessionId, object content)
        {
            var shareMessage = new ShareMessageCollection();
            var responseObject = new ResponseObject(content, sessionId);
            shareMessage.Set<IResponseObject>(responseObject);

            BatchExecutionContext context = new BatchExecutionContext()
            {
                SessionId = sessionId,
                ShareMessage = shareMessage
            };

            return context;
        }

        private class modelTest{
            public string Name { get; set; }
            public string Surname { get; set; }

        }

    }
}
