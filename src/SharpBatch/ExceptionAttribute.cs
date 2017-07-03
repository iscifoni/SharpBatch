using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch
{
    public class ExceptionAttribute : Attribute, IBatchExecutionAttribute
    {
        public int Order { get; set; } = -512;

        public void onExecuted(BatchExecutionContext context)
        {
        }

        public void onExecuting(BatchExecutionContext context)
        {
            var batchTraking = (ISharpBatchTracking)context.RequestServices.GetService(typeof(ISharpBatchTracking));
            var responseObject = context.ShareMessage.Get<IResponseObject>();
            var trakingModel = batchTraking.GetStatusAsync(context.SessionId)?.Result ;

            trakingModel.Ex.Add(responseObject.Response as Exception);
            trakingModel.State = StatusEnum.Error;

        }
    }
}
