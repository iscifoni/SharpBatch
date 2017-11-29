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
