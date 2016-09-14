using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ConfigAttributeExecutor
    {
        public async Task execute(BatchActionDescriptor action)
        {
            var invokeContext = new BatchConfigContext();
            invokeContext.BatchConfiguration = action.BatchConfiguration;

            for ( var i=0; i < action.ConfigureAttribute.Count;i++ )
            {
                await invoke(action.ConfigureAttribute[i], invokeContext);
            }
        }

        private Task invoke(IBatchConfigAttributeAsync attribute, BatchConfigContext context)
        {
            attribute.onExecuted(context);

            attribute.onExecuting(context);

            return Task.CompletedTask;
        }
    }
}
