using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public interface IBatchActionProvider
    {
        Task<string> InvokeAsync(BatchUrlManager urlManager, ContextInvoker context);
    }
}
