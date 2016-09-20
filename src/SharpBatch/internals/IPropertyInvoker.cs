using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public interface IPropertyInvoker
    {
        Task invokeAsync(object obj, ContextInvoker context);
    }
}
