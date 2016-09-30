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
