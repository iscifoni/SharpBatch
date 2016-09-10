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
    }
}
