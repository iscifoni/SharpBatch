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
    public static class DefaultActionDescriptionProvider
    {
        public static IEnumerable<BatchActionDescriptor> actionDescription(IEnumerable<TypeInfo> types)
        {
            List<BatchActionDescriptor> result = new List<BatchActionDescriptor>();
            foreach(var type in types)
            {
                var candidateToBatch = CandidateToBatch.getBatchActionDescription(type);
                if (candidateToBatch.Count() > 0)
                {
                    result.AddRange (candidateToBatch);
                }
            }
            return result;
        }

        internal static class CandidateToBatch
        {
            private static string cleanBatchName(string name)
            {
                var cleanName = name;
                if (cleanName.EndsWith("batch",StringComparison.OrdinalIgnoreCase))
                {
                    cleanName = cleanName.Substring(0, name.Length - "batch".Length);
                }

                return cleanName;
            }

            public static IList<BatchActionDescriptor> getBatchActionDescription(TypeInfo typeInfo)
            {
                IList<BatchActionDescriptor> response = new List<BatchActionDescriptor>();

                if (!(typeInfo.BaseType is object) && typeInfo.BaseType != null)
                {
                    response = getBatchActionDescription(typeInfo.BaseType.GetTypeInfo() );
                }

                if (isBatch(typeInfo))
                {
                    //Verify the Context Presence
                    PropertyInfo[] properties = typeInfo.GetProperties();
                    var propertyDiscovery = PropertyContextAttributeDiscovery.Create(properties);
                    var propertyDiscovered = propertyDiscovery.execute(properties);
                    foreach(var action in typeInfo.DeclaredMethods)
                    {
                       
                        if (isMethod(action))
                        {
                            var batchActionDescriptor = new BatchActionDescriptor()
                            {
                                Id = typeInfo.Name,
                                BatchName = cleanBatchName(typeInfo.Name),
                                BatchTypeInfo = typeInfo,
                                ActionName = action.Name,
                                ActionInfo = action,
                                PropertyInfo = propertyDiscovered
                            };

                            
                            batchActionDescriptor.ConfigureAttribute = new List<IBatchConfigAttributeAsync>();
                            batchActionDescriptor.ConfigureAttribute.AddRange(typeInfo.GetCustomAttributes<BatchConfigAttribute>(true));
                            batchActionDescriptor.ConfigureAttribute.AddRange(action.GetCustomAttributes<BatchConfigAttribute>(true));

                            executeConfigureAttribute(ref batchActionDescriptor);

                            if (batchActionDescriptor != null)
                            {
                                response.Add(batchActionDescriptor);
                            }
                        }
                    }
                }
                return response;
            }

            public static bool isBatch(TypeInfo typeInfo)
            {
                if (!typeInfo.IsClass)
                {
                    return false;
                }

                if (typeInfo.Name.EndsWith("Batch", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (typeInfo.GetCustomAttributes<BatchAttribute>(true).Count() > 0 )
                {
                    return true;
                }

                return false;
            }

            public static bool isMethod(MethodInfo methodInfo)
            {
                if (methodInfo.IsPrivate || 
                    methodInfo.IsAbstract ||
                    methodInfo.IsConstructor )
                {
                    return false;
                }

                if (methodInfo.GetCustomAttributes<BatchActionAttribute>(true).Count() > 0)
                {
                    return true;
                }

                return true;
            }

            public static void executeConfigureAttribute(ref BatchActionDescriptor action)
            {
                if (action.ConfigureAttribute.Count() > 0)
                {
                    var executor = new ConfigAttributeExecutor();
                    executor.execute(ref action);
                }
            }

        }

    }
}
