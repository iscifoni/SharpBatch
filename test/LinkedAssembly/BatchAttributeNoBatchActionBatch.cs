using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;

namespace LinkedAssemblyTest
{
    public class BatchAttributeNoBatchActionBatch
    {
        [NoBatchAction]
        public string test1()
        {
            return "test1";
        }
    }
}
