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
using System.Threading;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    internal class DefaultBatchInvokerProvider:IBatchInvokerProvider
    {
        private IBatchInvoker _batchInvoker;
        private ISharpBatchTraking _batchTraking;

        public DefaultBatchInvokerProvider(
            IBatchInvoker batchInvoker,
            ISharpBatchTrakingFactory batchTrakingFactory)
        {
            if (batchInvoker== null)
            {
                throw new ArgumentNullException(nameof(batchInvoker));
            }

            if (batchTrakingFactory == null)
            {
                throw new ArgumentNullException(nameof(batchTrakingFactory));
            }

            _batchInvoker = batchInvoker;
            _batchTraking = batchTrakingFactory.getTrakingProvider();
        }

        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;

            var sessionId = context.SessionId;

            await _batchTraking.StartAsync(sessionId);
            var cancellationToken = new CancellationToken();
            Task<object> task = Task.Run(async () =>
            {
                return await _batchInvoker.InvokeAsync(context);
            }, cancellationToken);

            TimerCallback timerAction = new TimerCallback(async (a) =>
            {
                await _batchTraking.PingAsync(sessionId);
            });
            Timer timer = new Timer(timerAction, null, new TimeSpan(0, 0, 2), new TimeSpan(0,0,2));

            var result = await task;
            timer.Dispose();

            await _batchTraking.StopAsync(sessionId);

            return result;
        }
    }
}
