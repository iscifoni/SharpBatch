using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class BatchExecutionAttribute : Attribute, IBatchExecutionAttribute
    {
        public int Order { get; set; }

        public void onExecuted(BatchExecutionContext context)
        {
            throw new NotImplementedException();
        }

        public void onExecuting(BatchExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
