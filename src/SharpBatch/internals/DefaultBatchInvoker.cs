using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace SharpBatch.internals
{
    public class DefaultBatchInvoker : IBatchInvoker
    {
        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var batchActionFactory = (IBatchActionFactory)context.RequestServices.GetService(typeof(IBatchActionFactory));
            var actionToExecute = batchActionFactory.Search(context.BatchName, context.ActionName);

            var parameters = await getInvokeParameters(actionToExecute);
            object targhet = Activator.CreateInstance(actionToExecute.BatchTypeInfo.AsType());

            var result = actionToExecute.ActionInfo.Invoke(targhet, parameters);

            if (result == null)
            {
                result = string.Empty;
            }

            context.Response.Body.Write(Encoding.UTF8.GetBytes(result.ToString()), 0, result.ToString().Length);

            return null;
        }

        private async Task<object[]> getInvokeParameters(BatchActionDescriptor actionDescription)
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
