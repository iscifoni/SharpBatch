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
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading;

namespace SharpBatch.internals
{
    internal class DefaultBatchHandler : IBatchHandler
    {
        IBatchActionFactory _batchActionFactory;

        public DefaultBatchHandler(IBatchActionFactory batchActionFactory)
        {
            _batchActionFactory = batchActionFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            BatchUrlManager urlManager = new BatchUrlManager(context.Request.Path);

            if ( urlManager.isBatch)
            {
                await InvokeAsync(context, urlManager);
            }
            else
            {
                //ToDo manage batch not found
            }
        }

        public async Task InvokeAsync(HttpContext context, BatchUrlManager urlManager)
        {
            await InvokeAsync(context, urlManager, null);
        }


        public async Task InvokeAsync(HttpContext context, BatchUrlManager urlManager, Guid? parentSessionId)
        {
            var contextInvoker = ContextInvoker.Create(context);
            contextInvoker.BatchName = urlManager.RequestBatchName;
            contextInvoker.ActionName = urlManager.RequestBatchAction;
            contextInvoker.ActionDescriptor = null;
            contextInvoker.ParentSessionID = parentSessionId;

            await InvokeAsync(contextInvoker, urlManager, parentSessionId);
        }

        public async Task InvokeAsync(ContextInvoker context, BatchUrlManager urlManager)
        {
            var contextInvoker = context;
            contextInvoker.BatchName = urlManager.RequestBatchName;
            contextInvoker.ActionName = urlManager.RequestBatchAction;
            contextInvoker.SessionId = Guid.NewGuid();
            contextInvoker.ActionDescriptor = null;

            await InvokeAsync(contextInvoker, urlManager, context.SessionId);

        }

        public async Task InvokeAsync(ContextInvoker context, BatchUrlManager urlManager, Guid? parentSessionId)
        {
            var batchActionProvider = _batchActionFactory.getProvider(urlManager);
            string response = await batchActionProvider.InvokeAsync(urlManager, context);
            response = $"{context.SessionId.ToString()} - {response}";
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(response), 0, response.Length);
        }

    }
}
