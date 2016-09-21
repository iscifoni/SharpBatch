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
                var batchActionDescriptions = AssemblyDiscoveryActionDescription.actionDescription(assembly.DefinedTypes);
                result.AddRange(batchActionDescriptions);
            }

            return result;
        }
    }
}
