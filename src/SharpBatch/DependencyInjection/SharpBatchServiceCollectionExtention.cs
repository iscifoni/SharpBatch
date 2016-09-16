using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharpBatch.internals;
using SharpBatch;
using SharpBatch.Traking.Abstraction;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharpBatchServiceCollectionExtention
    {
        public static IServiceCollection AddSharpBatch(this IServiceCollection services)
        {
            //Discovering batch
            var batchActionManager = getBatchAction(services);
            services.TryAddSingleton<ApplicationBatchManager>(batchActionManager);

            //invoker
            services.TryAddSingleton<IBatchInvoker, DefaultBatchInvoker>();
            services.TryAddSingleton<IBatchHandler, DefaultBatchHandler>();
            services.TryAddScoped<IBatchInvokerProvider, DefaultBatchInvokerProvider>();

            //batch Factory
            services.TryAddSingleton<IBatchActionFactory, BatchActionFactory>();
            services.TryAddSingleton<BatchActionProvider>();
            services.TryAddSingleton<SystemActionProvider>();

            //Traking
            services.TryAddSingleton<ISharpBatchTrakingFactory, SharpBatchTrakingFactory>();

            services.TryAddScoped<IBatchUtils, BatchUtils>();
            return services;
        }

        private static ApplicationBatchManager getBatchAction(IServiceCollection service)
        {
            var hostingService = (IHostingEnvironment)service.FirstOrDefault(t => t.ServiceType == typeof(IHostingEnvironment))?.ImplementationInstance;
            var manager = (ApplicationBatchManager)service.FirstOrDefault(t => t.ServiceType == typeof(ApplicationBatchManager))?.ImplementationInstance;

            if (manager == null)
            {
                manager = new ApplicationBatchManager();
            }

            var batchDescriptors = BatchActionDiscovery.discoveryBatchDescription(hostingService.ApplicationName);

            foreach(var item in batchDescriptors)
            {
                manager.BatchActions.Add(item);
            }

            return manager;
        }
         
    }
}
