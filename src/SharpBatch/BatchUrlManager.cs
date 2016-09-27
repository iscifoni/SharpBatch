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
using Microsoft.AspNetCore.Http;

namespace SharpBatch
{
    /// <summary>
    /// Dispatch a batch identified to a path request
    /// </summary>
    public class BatchUrlManager : IBatchUrlManager
    {
        private const string BaseBatch = "/batch";

        public BatchUrlManager()
        {
        }
        /// <summary>
        /// Initialize a new <see cref="BatchUrlManager"/>. 
        /// </summary>
        /// <param name="BatchName">Batch name</param>
        /// <param name="BatchActionName">Batch action name</param>
        public BatchUrlManager(string BatchName, string BatchActionName)
        {
            isBatch = true;
            RequestBatchAction = BatchActionName;
            RequestBatchName = BatchName;
        }
        /// <summary>
        /// Initialize a new <see cref="BatchUrlManager"/>
        /// </summary>
        /// <param name="path">Http request path</param>
        public BatchUrlManager(PathString path)
        {
            PathString batchUrlDetail;
            if (path.StartsWithSegments(new PathString(BaseBatch), out batchUrlDetail))
            {
                isBatch = true;

                var batchCallPathVector = batchUrlDetail.Value.Split('/');
                if (batchCallPathVector.Length >= 3)
                {
                    BatchUrlManagerCommand UrlManagerCommandEnumParser;
                    if(!Enum.TryParse<BatchUrlManagerCommand>(batchCallPathVector[1] , true , out UrlManagerCommandEnumParser))
                    {
                        throw new InvalidCastException($"Command {batchCallPathVector[1]} not found");
                    }

                    RequestCommand = UrlManagerCommandEnumParser;
                    RequestBatchName = batchCallPathVector[2];

                    //only for Exec command we have action to execute
                    RequestBatchAction = RequestCommand == BatchUrlManagerCommand.Exec ? batchCallPathVector[3] : null;
                    
                }
            }
        }

        /// <summary>
        /// Identify if url managed identify a batch
        /// </summary>
        public bool isBatch { get; private set; }

        /// <summary>
        /// Batch name identified
        /// </summary>
        public string RequestBatchName { get; private set; }

        /// <summary>
        /// Batch action identified
        /// </summary>
        public string RequestBatchAction { get; private set; }

        /// <summary>
        /// A <see cref="BatchUrlManagerCommand"/> 
        /// </summary>
        public BatchUrlManagerCommand RequestCommand { get; private set; }

    }
}
