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
using SharpBatch.internals;
using Microsoft.AspNetCore.Http;

namespace SharpBatch
{
    /// <summary>
    /// Utility class to manage chield Batch start.
    /// </summary>
    public class BatchUtils:IBatchUtils
    {
        private IBatchHandler _batchHandler;

        /// <summary>
        /// Initialize a new <see cref="BatchUtils"/>
        /// </summary>
        /// <param name="batchHandler"></param>
        public BatchUtils(IBatchHandler batchHandler )
        {
            _batchHandler = batchHandler;
        }

        /// <summary>
        /// Start nested batch action.
        /// </summary>
        /// <param name="batchName">Batch name</param>
        /// <param name="actionName">Action name</param>
        /// <param name="context">The <see cref="ContextInvoker"/> context</param>
        /// <returns>Return a <see cref="Task"/></returns>
        public Task startBatch(string batchName, string actionName, ContextInvoker context)
        {
            BatchUrlManager urlManager = new BatchUrlManager(batchName, actionName);
            return _batchHandler.InvokeAsync(context, urlManager);
        }

        /// <summary>
        /// Start nested batch action
        /// </summary>
        /// <param name="batchName">Batch name</param>
        /// <param name="actionName">Action name</param>
        /// <param name="context">The <see cref="BatchExecutionContext"/> context</param>
        /// <returns></returns>
        public Task startBatch(string batchName, string actionName, BatchExecutionContext context)
        {
            BatchUrlManager urlManager = new BatchUrlManager(batchName, actionName);
            
            var contextInvoker = ContextInvoker.Create(context.RequestServices, context.Request, context.Response);
            return _batchHandler.InvokeAsync(contextInvoker, urlManager);
        }
    }
}
