using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;
using System.Text;

namespace SharpBatch
{
    public class SharpBatchMiddleware
    {
        RequestDelegate _next;
        ILoggerFactory _loggerFactory;
        IBatchActionFactory _batchActionFactory;

        public SharpBatchMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IBatchActionFactory batchActionFactory
            )
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _batchActionFactory = batchActionFactory;
        }


        public async Task Invoke(HttpContext context)
        {
            PathString batchCallPath;
            //to do patternize it
            if (context.Request.Path.StartsWithSegments(new PathString("/batch/exec"), out batchCallPath))
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 3)
                {
                    var actionToExecute = _batchActionFactory.Search(batchCallPathVector[1], batchCallPathVector[2]);
                    object targhet = Activator.CreateInstance(actionToExecute.BatchTypeInfo.AsType());
                    var result = actionToExecute.ActionInfo.Invoke(targhet, null);
                    if (result == null)
                    {
                        result = string.Empty;
                    }
                    context.Response.Body.Write(Encoding.UTF8.GetBytes(result.ToString()), 0, result.ToString().Length);
                }
            }
            else 
            {
                await _next.Invoke(context);
            }
        }
    }
}
