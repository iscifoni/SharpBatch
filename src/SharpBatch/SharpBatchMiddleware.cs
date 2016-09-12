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
        IBatchHandler _batchHandler;

        public SharpBatchMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IBatchHandler batchHandler
            )
        {
            _next = next;
            _loggerFactory = loggerFactory;
            _batchHandler = batchHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            PathString batchCallPath;
            //to do patternize it using StringPath from config
            if (context.Request.Path.StartsWithSegments(new PathString("/batch"), out batchCallPath))
            {
                await _batchHandler.InvokeAsync(context);
            }
            else if (_next  != null )
            {
                await _next.Invoke(context);
            }
        }
    }
}
