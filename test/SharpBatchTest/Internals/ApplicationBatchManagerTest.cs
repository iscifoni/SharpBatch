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
using SharpBatch.internals;
using Xunit;

namespace SharpBatchTest.Internals
{
    public class ApplicationBatchManagerTest
    {
        [Fact]
        public void ApplicationBatchManager_SearcByNameAndAction_CaseInsernsitive()
        {
            //Arrange
            var batchManager = createApplicationBatchManager();

            //Act
            var response = batchManager.SearcByNameAndAction("batch1", "aCtion1");
            //Assert

            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(1, response.Count());
        }

        [Fact]
        public void ApplicationBatchManager_SearcByNameAndAction_CaseInsernsitive_NotFound()
        {
            //Arrange
            var batchManager = createApplicationBatchManager();

            //Act
            var response = batchManager.SearcByNameAndAction("batch11", "aCtion1");
            //Assert

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        private ApplicationBatchManager createApplicationBatchManager()
        {
            var batchManager = new ApplicationBatchManager();
            new ApplicationBatchManager();
            batchManager.BatchActions.Add(new BatchActionDescriptor()
            {
                BatchName = "Batch1",
                ActionName = "Action1"
            });
            batchManager.BatchActions.Add(new BatchActionDescriptor()
            {
                BatchName = "Batch1",
                ActionName = "Action2"
            });
            batchManager.BatchActions.Add(new BatchActionDescriptor()
            {
                BatchName = "Batch2",
                ActionName = "Action1"
            });

            return batchManager;
        }
    }
}
