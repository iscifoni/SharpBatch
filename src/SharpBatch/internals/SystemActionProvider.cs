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
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    public class SystemActionProvider : IBatchActionProvider
    {
        private ISharpBatchTraking _trakingProvider;
        public SystemActionProvider(ISharpBatchTrakingFactory trakingFactory)
        {
            _trakingProvider = trakingFactory.getTrakingProvider();
        }

        public async Task<string> InvokeAsync(IBatchUrlManager urlManager, ContextInvoker context)
        {
            switch (urlManager.RequestCommand)
                {
                case BatchUrlManagerCommand.Status:
                    var batchStaus = await _trakingProvider.GetStatus(new Guid(context.Parameters["sessionid"].ToString()));
                    var result = JSonSerializer.JSonModelSerializer.Serialize(batchStaus);
                    return result;
                default:
                    throw new InvalidCastException($"Command {urlManager.RequestCommand.ToString()} not found");
            }
        }
    }
}
