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
            await _batchUtil.startBatch("Class1", "Class1Method1", prop);
            return "Fatto";
        }

        [ResponseToFile(FileName ="ResponseFile", SessionIdInFileName =true, TimeStampTocken =true)]
        public string saveResponse()
        {
            return "Test to save";
        }

        [BatchContext]
        public ContextInvoker prop { get; set; }
    }
}
