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

            var dependencyContext =  DependencyContext.Load(currentAssembly);
            var notSystemLibrary = assemblyResolver.removeSystemAssembly(dependencyContext);

            var notSystemAssembly = notSystemLibrary
                        .SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext))
                        .Select(Assembly.Load);

            return notSystemAssembly; 
        }

    }
}
