using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

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
            var batchTraking = (ISharpBatchTraking)context.RequestServices.GetService(typeof(ISharpBatchTraking));
            var responseObject = context.ShareMessage.Get<IResponseObject>();
            var trakingModel = batchTraking.GetStatus(context.SessionId)?.Result ;

            trakingModel.Ex.Add(responseObject.Response as Exception);

        }
    }
}
