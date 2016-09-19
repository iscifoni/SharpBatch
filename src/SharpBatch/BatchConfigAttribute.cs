using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.internals;

namespace SharpBatch
{
    public class BatchConfigAttribute : Attribute, IBatchConfigAttributeAsync
    {
        private string _name;
        private object _value;

        public BatchConfigAttribute(string name, object value)
        {
            _name = name;
            _value = value;
        }

        public int Order { get; set; } = -15000;

        public virtual Task onExecuted(BatchConfigContext context)
        {
            return TaskWrapper.CompletedTask;
        }

        public virtual Task onExecuting(BatchConfigContext context)
        {
            context.BatchConfiguration.AddOrUpdate(_name, _value);
            return TaskWrapper.CompletedTask;
        }
    }
}
