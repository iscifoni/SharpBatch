using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch
{
    public class BatchExecutionAttribute : Attribute, IBatchExecutionAttribute
    {
        public virtual int Order { get; set; } = 1000;

        public virtual void onExecuted(BatchExecutionContext context)
        {
        }

        public virtual void onExecuting(BatchExecutionContext context)
        {
        }
    }
}
