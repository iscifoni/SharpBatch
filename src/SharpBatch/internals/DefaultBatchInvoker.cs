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

        //private readonly Func<Type, ObjectFactory> _batchActivator =
        //    (type) => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);

        public DefaultBatchInvoker(IPropertyInvoker propertyInvoker, MethodActivator activator)
        {
            _propertyInvoker = propertyInvoker;
            _activator = activator;
        }

        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;

            //Check Propertyes to activate 


            //Execute attribute onExecuting
            
            var parameterBinding = new DefaultBatchInvokerParameterBinding(context.Parameters, actionToExecute.ActionInfo);
            var parameters = parameterBinding.Bind();

            var executor = MethodExecutor.Create(actionToExecute.ActionInfo, actionToExecute.BatchTypeInfo);
            var activatorInstance = _activator.CreateInstance<object>(context.RequestServices, actionToExecute.BatchTypeInfo.AsType());
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
