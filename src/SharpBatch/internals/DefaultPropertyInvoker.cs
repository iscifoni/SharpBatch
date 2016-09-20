using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class DefaultPropertyInvoker : IPropertyInvoker
    {

        public Task invokeAsync(object obj, ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;
            actionToExecute.PropertyInfo.SetValue(obj, context);
            return TaskWrapper.CompletedTask;
        }
    }
}
