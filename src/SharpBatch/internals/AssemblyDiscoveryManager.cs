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
using Microsoft.Extensions.DependencyModel;

namespace SharpBatch.internals
{
    public static class AssemblyDiscoveryManager
    {
        private static class assemblyResolver
        {
            internal static HashSet<string> SystemAssembly { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "System",
                "Libuv",
                "Microsoft",
                "Newtonsoft",
                "NuGet",
                "NETStandard.Library",
                "runtime"
            };

            public static IEnumerable<RuntimeLibrary> removeSystemAssembly(DependencyContext dependencyContext)
            {
                //List<RuntimeLibrary> response = new List<RuntimeLibrary>();
                foreach(var library in dependencyContext.RuntimeLibraries)
                {
                    if (!isSystemLibrary(library))
                    {
                        yield return library;
                        //response.Add(library);
                    }
                }

                //return response;
            }

            private static bool isSystemLibrary(RuntimeLibrary library)
            {
                var p = SystemAssembly.Any(a => library.Name.StartsWith(a, StringComparison.OrdinalIgnoreCase));
                return p;
            }
        }


        public static IEnumerable<Assembly> EnlistAssemblyDependencies(string assemblyName)
        {
            var currentAssembly = Assembly.Load(new AssemblyName(assemblyName));
            IEnumerable<Assembly> notSystemAssembly = null;

            var dependencyContext =  DependencyContext.Load(currentAssembly);
            if (dependencyContext != null)
            {
                var notSystemLibrary = assemblyResolver.removeSystemAssembly(dependencyContext);

                notSystemAssembly = notSystemLibrary
                            .SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext))
                            .Select(Assembly.Load);
            }
            else
            {
                notSystemAssembly = new[] { currentAssembly };
            }

            return notSystemAssembly; 
        }

    }
}
