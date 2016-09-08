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
        IBatchInvoker _batchInvoker;
        private readonly PathString _batchStringIdentifier = new PathString("/batch/exec");
        
        public DefaultBatchHandler(IBatchInvoker batchInvoker)
        {
            _batchInvoker = batchInvoker;
        }

        public async Task<object> InvokeAsync(HttpContext context)
        {
            PathString batchCallPath;
            if (context.Request.Path.StartsWithSegments(_batchStringIdentifier, out batchCallPath))
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 3)
                {
                    var contextInvoker = ContextInvoker.Create(context);
                    contextInvoker.BatchName = batchCallPathVector[1];
                    contextInvoker.ActionName = batchCallPathVector[2];

                    var cancellationToken = new CancellationToken();
                    var task = Task.Run(async () =>
                        {
                            var response =  _batchInvoker.InvokeAsync(contextInvoker);
                        }, cancellationToken);

                    return task;
                }
            }

            return null;
        }
    }
}
