using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task onExecuted(BatchConfigContext context)
        {
            return Task.CompletedTask;
        }

        public Task onExecuting(BatchConfigContext context)
        {
            context.BatchConfiguration.AddOrUpdate(_name, _value);
            return Task.CompletedTask;
        }
    }
}
