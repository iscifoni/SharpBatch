using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public interface IBatchUtils
    {
        Task startBatch(string batchName, string actionName, ContextInvoker context);
    }
}
