using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;

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
            if (context.Request.Path.StartsWithSegments(new PathString("batch/exec/"),out batchCallPath) )
            {
                var batchCallPathVector = batchCallPath.Value.Split('/');
                if (batchCallPathVector.Length == 2)
                {
                    _batchActionFactory.Search(batchCallPathVector[0], batchCallPathVector[1]);
                }
            }
            await _next.Invoke(context);
        }
    }
}
