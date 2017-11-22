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
    public class BatchExecutionContext
    {
        public Guid SessionId { get; set; }

        public BatchActionDescriptor ActionDescriptor { get; set; }

        public BatchConfigurationDictionary Configuration { get; set; }

        public HttpRequest Request { get; set; }

        public HttpResponse Response { get; set; }

        public IShareMessageCollection ShareMessage { get; set; }

        public IServiceProvider RequestServices { get; set; }

        public static BatchExecutionContext Create(ContextInvoker context)
        {
            var executionContext = new BatchExecutionContext();
            executionContext.ActionDescriptor = context.ActionDescriptor;
            //executionContext.Configuration = context.ActionDescriptor.BatchConfiguration;
            executionContext.Request = context.Request;
            executionContext.Response = context.Response;
            executionContext.ShareMessage = context.ShareMessage;
            executionContext.RequestServices = context.RequestServices;
            executionContext.SessionId = context.SessionId;

            return executionContext;
        }
    }
}
