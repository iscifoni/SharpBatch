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
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace SharpBatch.internals
{
    public class DefaultBatchInvoker : IBatchInvoker
    {
        IPropertyInvoker _propertyInvoker;
        MethodActivator _activator;
        
        public DefaultBatchInvoker(IPropertyInvoker propertyInvoker, MethodActivator activator)
        {
            _propertyInvoker = propertyInvoker;
            _activator = activator;
        }

        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;
            var executor = MethodExecutor.Create(actionToExecute.ActionInfo, actionToExecute.BatchTypeInfo);
            var activatorInstance = _activator.CreateInstance<object>(context.RequestServices, actionToExecute.BatchTypeInfo.AsType());

            //Check Propertyes to activate 
            if (actionToExecute.PropertyInfo != null)
            {
                await _propertyInvoker.invokeAsync(activatorInstance, context);
            }

            //Execute attribute onExecuting

            var parameterBinding = new DefaultBatchInvokerParameterBinding(context.Parameters, actionToExecute.ActionInfo);
            var parameters = parameterBinding.Bind();

            var result = executor.Execute(activatorInstance, parameters);
            
            var response = (object)null;
            
            if(actionToExecute.IsAsync)
            {
                var task = result as Task;
                await task;

                var responseType = result.GetType();
                var taskTType = responseType.GetGenericArguments()[0];
                var resultProperty = typeof(Task<>).MakeGenericType(taskTType).GetProperty("Result");
                response = resultProperty.GetValue(task);
            }
            else
            {
                response = result;
            }

            //Save response in ShareMessage
            IResponseObject responseObject = new ResponseObject(response, context.SessionId);
            context.ShareMessage.Set<IResponseObject>(responseObject);
            
            //executing attribute on executed

            var serializedResult = "";
            //If result not null i serialize it 
            if (result != null)
            {
                serializedResult = JSonSerializer.JSonModelSerializer.Serialize(response);
            }
            return serializedResult;

        }

        private Task checkStatus(bool isComplete)
        {
            var response = new Task(null);
            return response;
        }
        

        /*
         * macchina stati tutto async
         * 
         * Invoke attributeExecution onExecuting
         * 
         * invoke parameters
         * 
         * invoche action 
         * 
         * invoke attributeExecution OnExecuted
         * 
         */


    }
}
