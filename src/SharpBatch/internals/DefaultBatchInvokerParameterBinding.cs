using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharpBatch.JSonSerializer;

namespace SharpBatch.internals
{
    internal class DefaultBatchInvokerParameterBinding
    {
        private BatchParameterDictionary _parameters;
        private MethodInfo _methodInfo;

        public DefaultBatchInvokerParameterBinding(BatchParameterDictionary parameters, MethodInfo methodInfo)
        {
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
                    MethodInfo method = typeof(JSonModelSerializer).GetMethod("Deserialize");
                    MethodInfo genericMethod = method.MakeGenericMethod(item.ParameterType);
                    result[i] = genericMethod.Invoke(null, new object[] { (string)parameterValue });
                }
            }

            return result;
        }
    }
}
