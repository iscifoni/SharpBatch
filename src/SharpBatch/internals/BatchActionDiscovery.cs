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
            IEnumerable<Assembly> assemblyList = AssemblyDiscoveryManager.EnlistAssemblyDependencies(assemblyName).ToList<Assembly>();
            
            foreach(var assembly in assemblyList)
            {
                defaultActionDescriptionProvider.actionDescription(assembly.DefinedTypes);
            }
            
            return null;
        }
    }
}
