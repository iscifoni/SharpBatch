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
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch
{
    public class ExceptionAttribute : Attribute, IBatchExecutionAttribute
    {
        public int Order { get; set; } = -512;

        public void onExecuted(BatchExecutionContext context)
        {
        }

        public void onExecuting(BatchExecutionContext context)
        {
            var batchTraking = (ISharpBatchTracking)context.RequestServices.GetService(typeof(ISharpBatchTracking));
            var responseObject = context.ShareMessage.Get<IResponseObject>();
            var trakingModel = batchTraking.GetStatusAsync(context.SessionId)?.Result ;

            trakingModel.Ex.Add(responseObject.Response as Exception);
            trakingModel.State = StatusEnum.Error;

        }
    }
}
