﻿//Copyright 2016 Scifoni Ivano
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
