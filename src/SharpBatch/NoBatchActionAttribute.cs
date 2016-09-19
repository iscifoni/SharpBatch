using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;

namespace SharpBatch
{
    public class NoBatchActionAttribute : BatchConfigAttribute
    {
        public NoBatchActionAttribute()
            : base(BatchConfigurationFieldName.BatchActionName, "NoAction")
        {
        }

        public override Task onExecuting(BatchConfigContext context)
        {
            context.ActionDescriptor = null;
            return TaskWrapper.CompletedTask;
        }

        public new int Order { get; set; } = 60001;
    }
}
