using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SharpBatch.internals
{
    public interface IBatchHandler
    {
        Task InvokeAsync(HttpContext context);
        Task InvokeAsync(HttpContext context, BatchUrlManager urlManager);
        Task InvokeAsync(ContextInvoker context, BatchUrlManager urlManager);
        Task InvokeAsync(ContextInvoker context, BatchUrlManager urlManager, Guid? parentSessionId);
    }
}
