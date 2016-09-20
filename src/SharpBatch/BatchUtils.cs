using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;
using Microsoft.AspNetCore.Http;

namespace SharpBatch
{
    public class BatchUtils:IBatchUtils
    {
        private IBatchHandler _batchHandler;

        public BatchUtils(IBatchHandler batchHandler )
        {
            _batchHandler = batchHandler;
        }

        //Start nested Batch
        public async Task startBatch(string batchName, string actionName, ContextInvoker context)
        {
            BatchUrlManager urlManager = new BatchUrlManager(batchName, actionName);
            await _batchHandler.InvokeAsync(context, urlManager);
        }
    }
}
