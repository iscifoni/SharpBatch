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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;

namespace SharpBatch
{
    public class ContextInvoker
    {
        public BatchParameterDictionary Parameters { get; } = new BatchParameterDictionary();

        public static ContextInvoker Create(HttpContext context)
        {

            var contextInvoker = new ContextInvoker()
            {
                RequestServices = context.RequestServices,
                Request = context.Request,
                Response = context.Response,
                SessionId = Guid.NewGuid()
            };

            if (context.Request.QueryString.HasValue)
            {
                contextInvoker.Parameters.AddFromQueryString(context.Request.QueryString);
            }
            return contextInvoker;
        }

        public IServiceProvider RequestServices { get; private set; }
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }

        public string BatchName { get; set; }
        public string ActionName { get; set; }
        public BatchActionDescriptor ActionDescriptor { get; set; }

        public Guid SessionId { get; set; }
        public Guid? ParentSessionID { get; set; }
        public IShareMessageCollection ShareMessage { get; } = new ShareMessageCollection();

    }
}
