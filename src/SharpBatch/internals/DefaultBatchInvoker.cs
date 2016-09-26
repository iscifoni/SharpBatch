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
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    public class DefaultBatchInvoker : IBatchInvoker
    {
        IPropertyInvoker _propertyInvoker;
        MethodActivator _activator;
        ISharpBatchTraking _sharpBatchTraking;
        
        public DefaultBatchInvoker(IPropertyInvoker propertyInvoker, MethodActivator activator, ISharpBatchTrakingFactory trakingFactory )
        {
            if (propertyInvoker == null)
            {
                throw new ArgumentNullException(nameof(propertyInvoker));
            }

            if (activator == null)
            {
                throw new ArgumentNullException(nameof(activator));
            }

            if(trakingFactory == null)
            {
                throw new ArgumentNullException(nameof(trakingFactory));
            }

            _propertyInvoker = propertyInvoker;
            _activator = activator;
            _sharpBatchTraking = trakingFactory.getTrakingProvider();
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

            var batchExecutionContext = BatchExecutionContext.Create(context);
            //Execute attribute onExecuting

            foreach(var executionAttribute in  actionToExecute.ExecutionAttribute)
            {
                executionAttribute.onExecuting(batchExecutionContext);
            }

            var parameterBinding = new DefaultBatchInvokerParameterBinding(context.Parameters, actionToExecute.ActionInfo);
            var parameters = parameterBinding.Bind();

            var result = executor.Execute(activatorInstance, parameters);
            
            var response = (object)null;

            try
            {
                if (actionToExecute.IsAsync)
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
            }catch(Exception ex)
            {
                IResponseObject responseObject = new ResponseObject(ex, context.SessionId);
                context.ShareMessage.Set<IResponseObject>(responseObject);

                //
                var exceptionAttribute = actionToExecute.ExecutionAttribute.OfType<ExceptionAttribute>();
                foreach(var item in exceptionAttribute)
                {
                    item.onExecuting(batchExecutionContext);

                    item.onExecuted(batchExecutionContext);
                } 
            }

            foreach (var executionAttribute in actionToExecute.ExecutionAttribute)
            {
                executionAttribute.onExecuted(batchExecutionContext);
            }

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

    }
}
