//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
            services.TryAddScoped<IPropertyInvoker, DefaultPropertyInvoker>();
            services.TryAddScoped<MethodActivator>();

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
