using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading;

namespace SharpBatch.internals
{
    internal class DefaultBatchHandler : IBatchHandler
    {
        IBatchActionFactory _batchActionFactory;

        public DefaultBatchHandler(IBatchActionFactory batchActionFactory)
        {
            _batchActionFactory = batchActionFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            BatchUrlManager urlManager = new BatchUrlManager(context.Request.Path);

            if ( urlManager.isBatch)
            {
                await InvokeAsync(context, urlManager);
            }
            else
            {
                //ToDo manage batch not found
            }
        }

        public async Task InvokeAsync(HttpContext context, BatchUrlManager urlManager)
        {
            await InvokeAsync(context, urlManager, null);
        }
        public async Task InvokeAsync(HttpContext context, BatchUrlManager urlManager, Guid? parentSessionId)
        {
            var contextInvoker = ContextInvoker.Create(context);
            contextInvoker.BatchName = urlManager.RequestBatchName;
            contextInvoker.ActionName = urlManager.RequestBatchAction;
            contextInvoker.ActionDescriptor = null;
            contextInvoker.ParentSessionID = parentSessionId;

            var batchActionProvider = _batchActionFactory.getProvider(urlManager);
            string response = await batchActionProvider.InvokeAsync(urlManager, contextInvoker);
            response = $"{contextInvoker.SessionId.ToString()} - {response}";
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response), 0, response.Length);
        }
    }
}
