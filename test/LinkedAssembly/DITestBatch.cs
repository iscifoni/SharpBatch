using SharpBatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkedAssemblyTest
{
    public class DITestBatch
    {
        private IBatchUtils _batchUtil;
        public DITestBatch(IBatchUtils batchUtil)
        {
            _batchUtil = batchUtil;
        }

        public async Task<string> method1()
        {
            await _batchUtil.startBatch("Class1", "Class1Method1", null);
            return "Fatto";
        }

        [BatchContext]
        public string prop { get; set; }
    }
}
