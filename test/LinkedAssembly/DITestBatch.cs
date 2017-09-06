//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
