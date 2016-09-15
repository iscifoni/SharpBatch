using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class BatchTimeOutAttribute : BatchConfigAttribute
    {
        public BatchTimeOutAttribute(TimeSpan expireTo) 
            : base(BatchConfigurationFieldName.TimeOut, expireTo)
        {
        }
    }
}
