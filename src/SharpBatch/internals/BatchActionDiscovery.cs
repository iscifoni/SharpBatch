using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public static class BatchActionDiscovery
    {
        public static IList<BatchActionDescriptor> discoveryBatchDescription(string assemblyName)
        {
            List<BatchActionDescriptor> result = new List<BatchActionDescriptor>();

            IEnumerable<Assembly> assemblyList = AssemblyDiscoveryManager.EnlistAssemblyDependencies(assemblyName).ToList<Assembly>();
            
            foreach(var assembly in assemblyList)
            {
                var batchActionDescriptions = DefaultActionDescriptionProvider.actionDescription(assembly.DefinedTypes);
                result.AddRange(batchActionDescriptions);
            }

            return result;
        }
    }
}
