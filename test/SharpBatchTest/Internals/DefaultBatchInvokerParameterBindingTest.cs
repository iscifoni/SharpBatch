using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using Moq;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class DefaultBatchInvokerParameterBindingTest
    {
        //ToDo : array parameter value

        [Fact]
        public void DefaultBatchInvokerParameterBindingTest_Bind()
        {
            //Arange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOBatch).GetTypeInfo());
            var descriptions = AssemblyDiscoveryActionDescription.actionDescription(types);

            var batchParameterDictionary = new BatchParameterDictionary();
            batchParameterDictionary.Add("param1", "first param");

            var serializer = new Mock<IModelSerializer>(MockBehavior.Strict);

            var batchInvokerParameterBinding = new DefaultBatchInvokerParameterBinding(batchParameterDictionary, descriptions.Last().ActionInfo, serializer.Object);

            //Act
            var response = batchInvokerParameterBinding.Bind();

            //Assert
            Assert.NotNull(response);
            Assert.IsType<object[]>(response);
            Assert.Equal(1, response.Length);
            Assert.Equal("first param", response[0]);

        }


    }
}
