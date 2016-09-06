using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SharpBatch.internals
{
    public interface IBatchInvoker
    {
        Task<object> InvokeAsync(ContextInvoker context);

    }
}
