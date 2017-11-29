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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Tracking.Memory;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class BatchActionProviderTest
    {
        [Fact]
        public async void BatchActionProviderTest_SimplePOCOBatchMethod1Invoke()
        {
            //Arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());

            //act
            var actionDescriptions = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());
            var filteredActionDescription = actionDescriptions.Where(p => p.ActionName == "method1");


            var batchUrlManager = new Mock<IBatchUrlManager>(MockBehavior.Strict);
            batchUrlManager.Setup((s) => s.RequestCommand)
                .Returns(BatchUrlManagerCommand.Exec)
                .Verifiable();
            batchUrlManager.Setup((s) => s.isBatch).Returns(true).Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchName).Returns("SimplePOCO").Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchAction).Returns("method1").Verifiable();

            var context = new ContextInvoker()
            {
                ActionDescriptor = null,
                ActionName = "method1",
                BatchName = "SimplePOCO",
                SessionId = Guid.NewGuid()
            };
            
            var applicationBatchManager = new Mock<IApplicationBatchManager>(MockBehavior.Strict);
            applicationBatchManager.Setup((s) => s.SearcByNameAndAction("SimplePOCO", "method1")).Returns(filteredActionDescription).Verifiable();

            var batchInvokerProvider = new Mock<IBatchInvokerProvider>(MockBehavior.Strict);
            batchInvokerProvider.Setup((s) => s.InvokeAsync(context)).Returns(Task.FromResult((object)"method1")).Verifiable();
            
            BatchActionProvider batchActionProvider = new BatchActionProvider(applicationBatchManager.Object, batchInvokerProvider.Object);

            //Act
            var result = await batchActionProvider.InvokeAsync(batchUrlManager.Object, context);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("method1", result);
            Assert.Same(filteredActionDescription.First(), context.ActionDescriptor);
            batchInvokerProvider.Verify();
            applicationBatchManager.Verify();
        }

        [Fact]
        public async void BatchActionProviderTest_SimplePOCOBatchMultipleMethodMatch()
        {
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());

            //act
            var actionDescriptions = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());


            var batchUrlManager = new Mock<IBatchUrlManager>(MockBehavior.Strict);
            batchUrlManager.Setup((s) => s.RequestCommand)
                .Returns(BatchUrlManagerCommand.Exec)
                .Verifiable();
            batchUrlManager.Setup((s) => s.isBatch).Returns(true).Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchName).Returns("SimplePOCO").Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchAction).Returns("method1").Verifiable();

            var context = new ContextInvoker()
            {
                ActionDescriptor = null,
                ActionName = "method1",
                BatchName = "SimplePOCO",
                SessionId = Guid.NewGuid()
            };

            var applicationBatchManager = new Mock<IApplicationBatchManager>(MockBehavior.Strict);
            applicationBatchManager.Setup((s) => s.SearcByNameAndAction("SimplePOCO", "method1")).Returns(actionDescriptions).Verifiable();

            var batchInvokerProvider = new Mock<IBatchInvokerProvider>(MockBehavior.Strict);
            batchInvokerProvider.Setup((s) => s.InvokeAsync(context)).Returns(Task.FromResult((object)"method1")).Verifiable();

            BatchActionProvider batchActionProvider = new BatchActionProvider(applicationBatchManager.Object, batchInvokerProvider.Object);

            var response = await Assert.ThrowsAsync<Exception>(() => batchActionProvider.InvokeAsync(batchUrlManager.Object, context) );
            Assert.Equal("Too many batch satisfy the search", response.Message);

        }

        [Fact]
        public async void BatchActionProviderTest_SimplePOCOBatchNoMethodMatch()
        {
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());

            //act
            IEnumerable<BatchActionDescriptor> actionDescriptions = new BatchActionDescriptor[0];


            var batchUrlManager = new Mock<IBatchUrlManager>(MockBehavior.Strict);
            batchUrlManager.Setup((s) => s.RequestCommand)
                .Returns(BatchUrlManagerCommand.Exec)
                .Verifiable();
            batchUrlManager.Setup((s) => s.isBatch).Returns(true).Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchName).Returns("SimplePOCO").Verifiable();
            batchUrlManager.Setup((s) => s.RequestBatchAction).Returns("method1").Verifiable();

            var context = new ContextInvoker()
            {
                ActionDescriptor = null,
                ActionName = "method1",
                BatchName = "SimplePOCO",
                SessionId = Guid.NewGuid()
            };

            var applicationBatchManager = new Mock<IApplicationBatchManager>(MockBehavior.Strict);
            applicationBatchManager.Setup((s) => s.SearcByNameAndAction("SimplePOCO", "method1")).Returns(actionDescriptions).Verifiable();

            var batchInvokerProvider = new Mock<IBatchInvokerProvider>(MockBehavior.Strict);
            batchInvokerProvider.Setup((s) => s.InvokeAsync(context)).Returns(Task.FromResult((object)"method1")).Verifiable();

            BatchActionProvider batchActionProvider = new BatchActionProvider(applicationBatchManager.Object, batchInvokerProvider.Object);

            var response = await Assert.ThrowsAsync<Exception>(() => batchActionProvider.InvokeAsync(batchUrlManager.Object, context));
            Assert.Equal("No batch satisfy the search", response.Message);

        }
    }
}
