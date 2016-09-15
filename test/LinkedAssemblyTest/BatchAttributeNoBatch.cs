using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;

namespace LinkedAssemblyTest
{
    [NoBatch()]
    public class BatchAttributeNoBatch
    {
        public string test1()
        {
            return "BatchAttributeNoBatch-test1";
        }
    }
}
