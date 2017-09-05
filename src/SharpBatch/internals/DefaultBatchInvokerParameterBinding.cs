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
using SharpBatch.Serialization.Abstract;

namespace SharpBatch.internals
{
    public class DefaultBatchInvokerParameterBinding
    {
        private BatchParameterDictionary _parameters;
        private MethodInfo _methodInfo;
        private IModelSerializer _modelSerializer;

        public DefaultBatchInvokerParameterBinding(BatchParameterDictionary parameters, MethodInfo methodInfo, IModelSerializer modelSerializer)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            _parameters = parameters;
            _methodInfo = methodInfo;
        }

        public object[] Bind()
        {
            var methodParameters = _methodInfo.GetParameters();
            object[] result = new object[methodParameters.Length];

            for(var i= 0; i< methodParameters.Length;i++)
            {
                var item = methodParameters[i];
                var parameterValue = _parameters[item.Name];

                if(item.ParameterType.Namespace.Equals("System", StringComparison.OrdinalIgnoreCase))
                {
                    result[i] = Convert.ChangeType(parameterValue, item.ParameterType);
                }else
                {
                    MethodInfo method = typeof(IModelSerializer).GetMethod("Deserialize");
                    MethodInfo genericMethod = method.MakeGenericMethod(item.ParameterType);
                    result[i] = genericMethod.Invoke(null, new object[] { (string)parameterValue });
                }
            }

            return result;
        }
    }
}
