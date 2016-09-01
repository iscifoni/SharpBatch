using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.DependencyInjection
{
    public static class SharpBatchBuilderExtention
    {
        public static IApplicationBuilder UseSharpBatch(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SharpBatchMiddleware>();
        }
    }
}
