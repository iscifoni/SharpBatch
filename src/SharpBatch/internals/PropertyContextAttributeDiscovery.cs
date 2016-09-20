using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public static class PropertyContextAttributeDiscovery
    {
        private delegate PropertyInfo PropertyDiscovery(PropertyInfo[] properties);

        private static PropertyDiscovery GetExecutor(PropertyInfo[] propertyInfo)
        {
            //return propertyInfo.Where(p => p.GetSetMethod().GetCustomAttribute(typeof(BatchContextAttribute)) != null).Select(m=>m);
            return (properties) => {
                PropertyInfo contextFind = null;

                foreach (var property in properties)
                {
                    var methodSet = property.GetSetMethod();
                    var attribute = methodSet.GetCustomAttribute(typeof(BatchContextAttribute));

                    if (attribute != null)
                        return property;
                }

                return contextFind;
            };
        }
    }
}
