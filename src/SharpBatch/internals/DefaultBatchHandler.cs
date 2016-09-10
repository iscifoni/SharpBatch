using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading;

namespace SharpBatch.internals
{
    public class DefaultBatchHandler : IBatchHandler
    {
        IBatchInvokerProvider _batchInvokerProvider;
        IBatchActionFactory _batchActionFactory;
        private readonly PathString _batchStringIdentifier = new PathString("/batch/exec");
        
        public DefaultBatchHandler(IBatchInvokerProvider batchInvokerProvider, IBatchActionFactory batchActionFactory)
        {
            _batchInvokerProvider = batchInvokerProvider;
            _batchActionFactory = batchActionFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            PathString batchCallPath;
            if (context.Request.Path.StartsWithSegments(_batchStringIdentifier, out batchCallPath))
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 3)
                {
                    var batchActionDescriptor = _batchActionFactory.Search(batchCallPathVector[1], batchCallPathVector[2]);

                    if (batchActionDescriptor != null)
                    {
                        var contextInvoker = ContextInvoker.Create(context);
                        contextInvoker.BatchName = batchCallPathVector[1];
                        contextInvoker.ActionName = batchCallPathVector[2];
                        contextInvoker.ActionDescriptor = batchActionDescriptor;

                        var asyncCall = (bool)(batchActionDescriptor.BatchConfiguration["AsynCall"] ?? true);

                        if (asyncCall)
                        {
                            var response = _batchInvokerProvider.InvokeAsync(contextInvoker);
                            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Batch Started"), 0, "Batch Started".Length);
                        }else
                        {
                            var response = await _batchInvokerProvider.InvokeAsync(contextInvoker) as string;
                            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response), 0, response.Length);
                        }
                    }
                    else
                    {
                        //ToDo manage batch not found
                    }
                }
            }

        }
    }
}
