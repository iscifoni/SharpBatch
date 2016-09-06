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
        

        public async Task<object> InvokeAsync(HttpContext context)
        {
            var batchInvoker = (IBatchInvoker)context.RequestServices.GetService(typeof(IBatchInvoker));
            PathString batchCallPath;
            //to do patternize it
            if (context.Request.Path.StartsWithSegments(new PathString("/batch/exec"), out batchCallPath))
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 3)
                {
                    var contextInvoker = ContextInvoker.Create(context);
                    contextInvoker.BatchName = batchCallPathVector[1];
                    contextInvoker.ActionName = batchCallPathVector[2];

                    await batchInvoker.InvokeAsync(contextInvoker);
                }
            }
            return null;
        }
    }
}
