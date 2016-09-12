using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SharpBatch.internals;


namespace SharpBatch
{
    public class BatchConfigContext
    {
        public batchConfigurationDictionary BatchConfiguration { get; set; }
        public HttpRequest Request { get; set; }
    }
}
