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
    public  class PropertyContextAttributeDiscovery
    {
        private delegate PropertyInfo PropertyDiscovery(PropertyInfo[] properties);

        private PropertyDiscovery _executor;

        public PropertyContextAttributeDiscovery(PropertyInfo[] propertyInfo)
        {
            _executor = GetExecutor(propertyInfo);
        }

        private PropertyDiscovery GetExecutor(PropertyInfo[] propertyInfo)
        {
            //return propertyInfo.Where(p => p.GetSetMethod().GetCustomAttribute(typeof(BatchContextAttribute)) != null).Select(m=>m);
            return (properties) => {
                PropertyInfo contextFind = null;

                foreach (var property in properties)
                {
                    var attribute = property.GetCustomAttribute<BatchContextAttribute>(false);

                    if (attribute != null)
                        return property;
                }

                return contextFind;
            };
        }

        public PropertyInfo execute(PropertyInfo[] properties)
        {
            return _executor(properties);
        }

        public static PropertyContextAttributeDiscovery Create(PropertyInfo[] propertyInfo)
        {
            var executor = new PropertyContextAttributeDiscovery(propertyInfo);
            //executor._executor = GetExecutor(propertyInfo);
            return executor ;
        }
    }
}
