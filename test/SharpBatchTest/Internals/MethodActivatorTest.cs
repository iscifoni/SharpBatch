using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SharpBatch;
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class MethodActivatorTest
    {
        [Fact]
        public void MethodActivator_CreateInstanceWithParameters()
        {
            //Arrange
            var methodActivator = new MethodActivator();
            var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProvider.Setup((s) => s.GetService(It.IsAny<Type>()))
                .Returns(new SimplePOCOBatch());

            var implementationType = typeof(SimplePOCOContructorParamsBatch);
            //Act
            var response = methodActivator.CreateInstance<object>(serviceProvider.Object, implementationType);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<SimplePOCOContructorParamsBatch>(response);
        }

        [Fact]
        public void MethodActivator_CreateInstanceWithoutParameters()
        {
            //Arrange
            var methodActivator = new MethodActivator();
            var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            serviceProvider.Setup((s) => s.GetService(It.IsAny<Type>()));

            var implementationType = typeof(SimplePOCOBatch);
            //Act
            var response = methodActivator.CreateInstance<object>(serviceProvider.Object, implementationType);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<SimplePOCOBatch>(response);
        }

    }
}
