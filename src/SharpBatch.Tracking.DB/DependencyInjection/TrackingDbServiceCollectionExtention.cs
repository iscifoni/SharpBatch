using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharpBatch.Tracking.Abstraction;
using SharpBatch.Tracking.DB;
using SharpBatch.Tracking.DB.data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TrackingDbServiceCollectionExtention
    {
        public static IServiceCollection AddSharpBatchTrackingDB(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<TrackingContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ISharpBatchTracking, TrackingDb>();

            return services;
        }
    }
}
