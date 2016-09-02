using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ApplicationBatchManager
    {
        public IList<BatchActionDescriptor> BatchActions { get; } = new List<BatchActionDescriptor>();


    }
}
