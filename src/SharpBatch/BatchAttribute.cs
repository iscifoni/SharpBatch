using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class BatchAttribute: Attribute, IBatchAttribute
    {
        public string BatchName { get; set; }

    }
}
