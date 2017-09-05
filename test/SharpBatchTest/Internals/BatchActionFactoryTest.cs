﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Memory;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class BatchActionFactoryTest
    {
        [Theory]
        [InlineData(BatchUrlManagerCommand.Exec)]
        [InlineData(BatchUrlManagerCommand.Status)]
        public void BatchActionFactoryTest_getProvider(BatchUrlManagerCommand command)
        {
            //Arrange
            var sharpBatchTrakingFactory = new Mock<ISharpBatchTrackingFactory>(MockBehavior.Strict);
            sharpBatchTrakingFactory.Setup((s) => s.getTrakingProvider())
                .Returns(new TrackingMemory())
                .Verifiable();

            var modelSerializer = new Mock<IModelSerializer>(MockBehavior.Strict);

            IPropertyInvoker propertyInvoker = new DefaultPropertyInvoker();
            MethodActivator methodActivator = new MethodActivator();
            IBatchInvoker batchInvoker = new DefaultBatchInvoker(propertyInvoker, methodActivator, sharpBatchTrakingFactory.Object, modelSerializer.Object);
            IBatchInvokerProvider batchInvokerProvider = new DefaultBatchInvokerProvider(batchInvoker,sharpBatchTrakingFactory.Object);
            ApplicationBatchManager applicationBatchManager = new ApplicationBatchManager();
            BatchActionProvider batchActionProvider = new BatchActionProvider(applicationBatchManager, batchInvokerProvider);

            SystemActionProvider systemActionProvider = new SystemActionProvider(sharpBatchTrakingFactory.Object, modelSerializer.Object);
            IBatchActionFactory batchActionFactory = new BatchActionFactory(batchActionProvider, systemActionProvider);

            var batchUrlManager = new Mock<IBatchUrlManager>(MockBehavior.Strict);
            batchUrlManager.Setup((s)=> s.RequestCommand)
                .Returns(command)
                .Verifiable();


            //Act
            var actionProvider = batchActionFactory.getProvider(batchUrlManager.Object);


            //Assert
            if (command == BatchUrlManagerCommand.Exec)
            {
                Assert.Same(batchActionProvider, actionProvider);
            }
            else
            {
                Assert.Same(systemActionProvider, actionProvider);
            }
            batchUrlManager.Verify();
            sharpBatchTrakingFactory.Verify();
        }
    }
}
