using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharpBatch.internals;

namespace SharpBatch.DependencyInjection
{
    public static class SharpBatchServiceCollectionExtention
    {
        public static IServiceCollection AddSharpBatch(this IServiceCollection service)
        {
            service.AddSingleton<IBatchActionFactory, BatchActionFactory>();
            return service;
        }

         
    }
}
