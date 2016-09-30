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
    public class ConfigAttributeExecutorTest
    {
        [Fact]
        public void ConfigAttributeExecutorTest_Execute()
        {
            //Arrange
            List<TypeInfo> types = new List<TypeInfo>();
            types.Add(typeof(SimplePOCOConfgigurationAttributeBatch).GetTypeInfo());
            var actionsDescriptor = AssemblyDiscoveryActionDescription.actionDescription(types.AsEnumerable());
            BatchActionDescriptor action = actionsDescriptor.First();

            var configureAttributeExecutor = new ConfigAttributeExecutor();

            //Act
           configureAttributeExecutor.execute(ref action);

            //Assert
            Assert.Equal(action.BatchName, "BatchName");
        }
    }
}
