using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class DefaultPropertyInvokerTest
    {
        [Fact]
        public async void DefaultPropertyInvokerTest_invoke()
        {
            //Arrange
            DefaultPropertyInvoker propertyInvoker = new DefaultPropertyInvoker();
            SimplePOCOPropertyBatch simplePOCO = new SimplePOCOPropertyBatch();
            ContextInvoker contextInvoker = new ContextInvoker()
            {
                ActionDescriptor = new BatchActionDescriptor()
            };
            contextInvoker.ActionDescriptor.PropertyInfo = typeof(SimplePOCOPropertyBatch).GetTypeInfo().GetDeclaredProperty("context");

            //Act
            await propertyInvoker.invokeAsync(simplePOCO, contextInvoker);

            //Assert
            Assert.NotNull(simplePOCO.context);

        }
    }
}
