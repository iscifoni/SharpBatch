using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch
{
    public class BatchExceptionAttribute : Attribute, IBatchExceptionAttribute
    {
        public virtual int Order { get; set; }

        public virtual void onExecuted(BatchExecutionContext context, Exception ex)
        {
        }
    }
}
