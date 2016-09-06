using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace SharpBatch.internals
{
    public class DefaultBatchHandler : IBatchHandler
    {
        private readonly PathString batchStringIdentifier = new PathString("/batch/exec");
        

        public async Task<object> InvokeAsync(HttpContext context)
        {
            var batchInvoker = (IBatchInvoker)context.RequestServices.GetService(typeof(IBatchInvoker));
            PathString batchCallPath;
            if (context.Request.Path.StartsWithSegments(batchStringIdentifier, out batchCallPath))
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 3)
                {
                    var contextInvoker = ContextInvoker.Create(context);
                    contextInvoker.BatchName = batchCallPathVector[1];
                    contextInvoker.ActionName = batchCallPathVector[2];

                    var task = Task.Run(()=>
                        {
                            var response = batchInvoker.InvokeAsync(contextInvoker);
                        }
                    );
                    
                    return true;
                }
            }

            return false;
        }
    }
}
