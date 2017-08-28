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
using SharpBatch;
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class BatchActionDescriptorTest
    {
        [Fact]
        public void BatchActionDescriptorTest_BatchNameActionNamePropertyChange()
        {
            //arrange
            var batchActionDescriptor = new BatchActionDescriptor();

            //act
            batchActionDescriptor.BatchName = "batchname";
            batchActionDescriptor.ActionName = "actionname";

            //assert
            Assert.Collection<KeyValuePair<string, object>>(batchActionDescriptor.BatchConfiguration.AsEnumerable<KeyValuePair<string, object>>(),
                (p) => 
                {
                    Assert.IsType<string>(p.Value);
                    Assert.Equal("batchname", (string)p.Value);
                },
                (p) => 
                {
                    Assert.IsType<string>(p.Value);
                    Assert.Equal("actionname", (string)p.Value);
                }
            );
        }

        [Fact]
        public void BatchActionDescriptorTest_BatchNameActionNameConfigurationChange()
        {
            //arrange
            var batchActionDescriptor = new BatchActionDescriptor();

            //act
            batchActionDescriptor.BatchConfiguration.Add(new KeyValuePair<string, object>(BatchConfigurationFieldName.BatchName, "batchname"));
            batchActionDescriptor.BatchConfiguration.Add(new KeyValuePair<string, object>(BatchConfigurationFieldName.BatchActionName, "actionname"));
            batchActionDescriptor.refreshBatchNameAndBatchAction();

            //assert
            Assert.Equal("batchname", batchActionDescriptor.BatchName);
            Assert.Equal("actionname", batchActionDescriptor.ActionName);
        }
    }
}
