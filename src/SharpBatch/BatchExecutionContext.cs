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
        BatchActionDescriptor Action { get; set; }

        batchConfigurationDictionary Configuration { get; set; }

        HttpRequest Request { get; set; }

        HttpResponse Response { get; set; }

        IShareMessageCollection ShareMessage { get; set; }

        BatchUrlManager urlManager { get; set; }
    }
}
