using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class BatchAttribute: BatchConfigAttribute
    {
        public BatchAttribute(object value) 
            : base(BatchConfigurationFieldName.BatchName, value)
        {
        }

    }
}
