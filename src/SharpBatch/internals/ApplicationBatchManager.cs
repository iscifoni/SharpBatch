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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class ApplicationBatchManager : IApplicationBatchManager
    {
        public IList<BatchActionDescriptor> BatchActions { get; } = new List<BatchActionDescriptor>();
        
        public IEnumerable<BatchActionDescriptor> SearcByNameAndAction(string BatchName, string BatchAction)
        {
            for(var i=0;i<BatchActions.Count(); i++)
            {
                var item = BatchActions[i];

                if (item.BatchName.Equals(BatchName, StringComparison.OrdinalIgnoreCase)
                    && item.ActionName.Equals(BatchAction, StringComparison.OrdinalIgnoreCase) )
                {
                    yield return item;
                }
            }
        }
    }
}
