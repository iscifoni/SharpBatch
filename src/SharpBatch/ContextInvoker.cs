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
