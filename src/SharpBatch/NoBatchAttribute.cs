using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class NoBatchAttribute : BatchConfigAttribute
    {
        public NoBatchAttribute() 
            : base(BatchConfigurationFieldName.BatchName, "NoBatch")
        {
        }

        public override Task onExecuting(BatchConfigContext context)
        {
            context.ActionDescriptor = null;
            return Task.CompletedTask;
        }

        public new int Order { get; set; } = 60000;
    }
}
