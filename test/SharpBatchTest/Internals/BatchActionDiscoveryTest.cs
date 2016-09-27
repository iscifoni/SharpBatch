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
            Assert.Equal(8, response.Count());
            Assert.Collection(response,
                (p) => {
                    Assert.Equal(p.BatchName, "SimplePOCO");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "SimplePOCO");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                },
                 (p) => {
                     Assert.Equal(p.BatchName, "InferitFromSimplePOCO");
                     Assert.Equal(p.ActionName, "method1");
                     Assert.Equal(p.ReturnType, typeof(string));
                 },
                (p) => {
                    Assert.Equal(p.BatchName, "InferitFromSimplePOCO");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "BatchFromAttribute");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "BatchFromAttribute");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "InheritFromBatchAttribute");
                    Assert.Equal(p.ActionName, "method1");
                    Assert.Equal(p.ReturnType, typeof(string));
                },
                (p) => {
                    Assert.Equal(p.BatchName, "InheritFromBatchAttribute");
                    Assert.Equal(p.ActionName, "method2");
                    Assert.Equal(p.ReturnType, typeof(int));
                }
            );
        }
        
    }
}
