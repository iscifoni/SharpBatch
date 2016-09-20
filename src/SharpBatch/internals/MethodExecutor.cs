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
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class MethodExecutor
    {
        private ActionExecutor _executor;
        
        private delegate object ActionExecutor(object target, object[] parameters);
        private delegate void VoidActionExecutor(object target, object[] parameters);
        
        public MethodExecutor(MethodInfo methodInfo, TypeInfo targetTypeInfo)
        {
            _executor = GetExecutor(methodInfo, targetTypeInfo);
        }
        
        public static MethodExecutor Create(MethodInfo methodInfo, TypeInfo targetTypeInfo)
        {
            var executor = new MethodExecutor(methodInfo, targetTypeInfo);
            executor._executor = GetExecutor(methodInfo, targetTypeInfo);
            return executor;
        }

        private static ActionExecutor GetExecutor(MethodInfo methodInfo, TypeInfo typeInfo)
        {
            // Parameters to executor
            var parameter = Expression.Parameter(typeof(object), "target");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            
            var parameters = new List<Expression>();
            var methodParameters = methodInfo.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var paramenterInfo = methodParameters[i];
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramenterInfo.ParameterType);
                parameters.Add(valueCast);
            }

            var instance = Expression.Convert(parameter, typeInfo.AsType());
            var methodCall = Expression.Call(instance, methodInfo, parameters);

            if (methodCall.Type == typeof(void))
            {
                var lambda = Expression.Lambda<VoidActionExecutor>(methodCall, parameter, parametersParameter);
                var voidExecutor = lambda.Compile();
                return VoidAction(voidExecutor);
            }
            else
            {
                // must coerce methodCall to match ActionExecutor signature
                var castMethodCall = Expression.Convert(methodCall, typeof(object));
                var lambda = Expression.Lambda<ActionExecutor>(castMethodCall, parameter, parametersParameter);
                return lambda.Compile();
            }
        }

        private static ActionExecutor VoidAction(VoidActionExecutor executor)
        {
            return delegate (object target, object[] parameters)
            {
                executor(target, parameters);
                return null;
            };
        }

        public object Execute(object target, object[] parameters)
        {
            return _executor(target, parameters);
        }
    }

}
