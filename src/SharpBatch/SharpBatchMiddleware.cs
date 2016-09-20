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
