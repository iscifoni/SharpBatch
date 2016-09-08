using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SharpBatch.internals
{
    public class DefaultBatchInvoker : IBatchInvoker
    {
        public object Invoke(ContextInvoker context)
        {
            var batchActionFactory = (IBatchActionFactory)context.RequestServices.GetService(typeof(IBatchActionFactory));
            var actionToExecute = batchActionFactory.Search(context.BatchName, context.ActionName);

            var parameters = getInvokeParameters(actionToExecute);
            object targhet = Activator.CreateInstance(actionToExecute.BatchTypeInfo.AsType());

            var result = actionToExecute.ActionInfo.Invoke(targhet, parameters);

            if (result == null)
            {
                result = string.Empty;
            }

            context.Response.Body.Write(Encoding.UTF8.GetBytes(result.ToString()), 0, result.ToString().Length);

            return null;
        }

        public async Task<object> InvokeAsync(ContextInvoker context)
        {

            var batchActionFactory = (IBatchActionFactory)context.RequestServices.GetService(typeof(IBatchActionFactory));
            var actionToExecute = batchActionFactory.Search(context.BatchName, context.ActionName);

            var parameters = getInvokeParameters(actionToExecute);
            object targhet = Activator.CreateInstance(actionToExecute.BatchTypeInfo.AsType());

            var result = await Task.Run(()=> actionToExecute.ActionInfo.Invoke(targhet, parameters));
            if ( isAsyncMethod(actionToExecute.ActionInfo))
            {

            }
            
            return result;
        }

        private Task checkStatus(bool isComplete)
        {
            var response = new Task(null);
            return response;
        }

        private bool isAsyncMethod(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<AsyncStateMachineAttribute>();
            return attribute == null;
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
