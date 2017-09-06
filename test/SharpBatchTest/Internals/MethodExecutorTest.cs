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
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class MethodExecutorTest
    {
        [Fact]
        public void MethodExecutorTest_ExecuteMethodWithoutParameter()
        {
            //Arrange
            var methodInfo = typeof(SimplePOCOBatch).GetMethod("method1");
            var targetTypeInfo = typeof(SimplePOCOBatch).GetTypeInfo();
            var instance = new SimplePOCOBatch();

            MethodExecutor methodExecutor = MethodExecutor.Create(methodInfo, targetTypeInfo);

            //Act
            var response = methodExecutor.Execute(instance, null);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<string>(response);
            Assert.Equal("method1", response);
        }

        [Fact]
        public void MethodExecutorTest_ExecuteMethodWithParameter()
        {
            //Arrange
            var methodInfo = typeof(SimplePOCOBatch).GetMethod("method3");
            var targetTypeInfo = typeof(SimplePOCOBatch).GetTypeInfo();
            var instance = new SimplePOCOBatch();

            MethodExecutor methodExecutor = MethodExecutor.Create(methodInfo, targetTypeInfo);

            //Act
            var response = methodExecutor.Execute(instance, new[] { "method3" });

            //Assert
            Assert.NotNull(response);
            Assert.IsType<string>(response);
            Assert.Equal("method3", response);
        }
    }
}
