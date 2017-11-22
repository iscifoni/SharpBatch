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
        ILogger<SharpBatchMiddleware> _logger;
        IBatchHandler _batchHandler;

        public SharpBatchMiddleware(
            RequestDelegate next,
            ILogger<SharpBatchMiddleware> logger,
            IBatchHandler batchHandler
            )
        {
            _next = next;
            _logger = logger;
            _batchHandler = batchHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            PathString batchCallPath;
            //to do patternize it using StringPath from config
            if (context.Request.Path.StartsWithSegments(new PathString("/batch"), out batchCallPath))
            {
                try
                {
                    await _batchHandler.InvokeAsync(context);
                }catch (BatchNotFoundException ex )
                {
                    _logger.LogError(ex, ex.Message);
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            }
            else if (_next  != null )
            {
                await _next.Invoke(context);
            }
        }
    }
}
