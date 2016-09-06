using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ContextInvoker
    {
        public static ContextInvoker Create(HttpContext context)
        {
            return new ContextInvoker()
            {
                RequestServices = context.RequestServices,
                Request = context.Request,
                Response = context.Response
            };
        }

        public IServiceProvider RequestServices { get; private set; }
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }

        public string BatchName { get; set; }
        public string ActionName { get; set; }

    }
}
