using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class BatchActionAttribute : BatchConfigAttribute
    {
        public BatchActionAttribute(string actionValue)
            :base(BatchConfigurationFieldName.BatchActionName, actionValue)
        {

        }
    }
}
