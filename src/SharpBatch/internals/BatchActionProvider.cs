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

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class BatchActionProvider : IBatchActionProvider
    {
        IApplicationBatchManager _applicationBatchManager;
        IBatchInvokerProvider _batchInvokerProvider;

        public BatchActionProvider(IApplicationBatchManager applicationBatchManager,
            IBatchInvokerProvider batchInvokerProvider)
        {
            if (applicationBatchManager == null)
            {
                throw new ArgumentNullException(nameof(applicationBatchManager));
            }

            if ( batchInvokerProvider == null)
            {
                throw new ArgumentNullException(nameof(batchInvokerProvider));
            }

            _applicationBatchManager = applicationBatchManager;
            _batchInvokerProvider = batchInvokerProvider;
        }

        public async Task<string> InvokeAsync(IBatchUrlManager urlManager, ContextInvoker context)
        {
            try
            {
                var batchActionDescriptor = Search(urlManager.RequestBatchName, urlManager.RequestBatchAction);
                context.ActionDescriptor = batchActionDescriptor;

                if (batchActionDescriptor.IsAsync)
                {
                    var response = _batchInvokerProvider.InvokeAsync(context);
                    return "Batch Started";
                }
                else
                {
                    var response = await _batchInvokerProvider.InvokeAsync(context) as string;
                    return response?.ToString();
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private BatchActionDescriptor Search(string BatchName, string ActionName)
        {
            var batchActionDescriptors = _applicationBatchManager.SearcByNameAndAction(BatchName, ActionName);

            if ( batchActionDescriptors.Count() > 1)
            {
                //To do custom exception
                throw new Exception("Too many batch satisfy the search");
            }

            if ( batchActionDescriptors.Count() == 0 )
            {
                //To do custom exception
                throw new Exception("No batch satisfy the search");
            }

            return batchActionDescriptors.First();
        }
    }
}
