using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ConfigAttributeExecutor
    {
        public void execute(ref BatchActionDescriptor action)
        {
            var invokeContext = new BatchConfigContext();
            invokeContext.BatchConfiguration = action.BatchConfiguration;
            invokeContext.ActionDescriptor = action;

            for ( var i=0; i < action.ConfigureAttribute.Count;i++ )
            {
                invoke(action.ConfigureAttribute[i], invokeContext);
            }

            action = invokeContext.ActionDescriptor;
        }

        private void invoke(IBatchConfigAttributeAsync attribute, BatchConfigContext context)
        {
            attribute.onExecuting(context);

            attribute.onExecuted(context);
        }
    }
}
