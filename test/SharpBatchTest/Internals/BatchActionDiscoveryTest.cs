using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class BatchActionDiscoveryTest
    {

        [Fact]
        public void BatchActionDiscoveryTest_actionDescription_DiscoveringAllMethod()
        {
            //arrange
            var assemblyName = typeof(BatchActionDiscoveryTest).GetTypeInfo().Assembly.GetName().Name;

            //act
            var response = BatchActionDiscovery.discoveryBatchDescription(assemblyName);

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(13, response.Count());
        }
        
    }
}
