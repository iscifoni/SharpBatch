﻿//Copyright 2016 Scifoni Ivano
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
using System.Text;
using SharpBatch.JSonSerializer;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch
{
    public class ResponseToTrackingAttribute : BatchExecutionAttribute
    {
        public override int Order { get; set; } = 10000;

        public override void onExecuted(BatchExecutionContext context)
        {
            var response = context.ShareMessage.Get<IResponseObject>();
            var responseType = response.Response.GetType();
            var trackingService = (ISharpBatchTracking)context.RequestServices.GetService(typeof(ISharpBatchTracking));
            
            trackingService.AddMessageAsync(context.SessionId, JSonModelSerializer.Serialize(response.Response));
        }

        public override void onExecuting(BatchExecutionContext context)
        {
        }
    }
}
