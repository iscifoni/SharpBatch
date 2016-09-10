using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SharpBatch.internals
{
    public class DefaultBatchInvoker : IBatchInvoker
    {
        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;

            var parameters = getInvokeParameters(actionToExecute);
            var parameterBinding = new DefaultBatchInvokerParameterBinding(context.Parameters, actionToExecute.ActionInfo);
            parameters = parameterBinding.Bind();

            object targhet = Activator.CreateInstance(actionToExecute.BatchTypeInfo.AsType());

            var result = actionToExecute.ActionInfo.Invoke(targhet, parameters);
            var response = (object)null;

            if (result is Task)
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

        private object[] getInvokeParameters(BatchActionDescriptor actionDescription)
        {
            IList<object> parameterResult = new List<object>();
            var parametersInfo = actionDescription.ActionInfo.GetParameters();
            for(var i = 0; i<parametersInfo.Length; i++)
            {
                var item = parametersInfo[i];
                if (item.IsIn)
                {
                    parameterResult.Add(item);
                }
            }

            return null;
        }

        


    }
}
