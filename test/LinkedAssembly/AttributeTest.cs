using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;

namespace LinkedAssemblyTest
{
    [Batch("Pippo")]
    public class AttributeTest
    {
        [BatchAction("vai")]
        public string go()
        {
            return "Started";
        }
    }
}
