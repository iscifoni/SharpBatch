using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch
{
    public interface IBatchExceptionAttribute
    {
        int Order { get; set; }

        void onExecuted(BatchExecutionContext context);
    }
}
