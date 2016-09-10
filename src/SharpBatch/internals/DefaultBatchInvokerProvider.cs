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
            _batchInvoker = batchInvoker;
            _batchTraking = batchTrakingFactory.getTrakingProvider();
        }

        public async Task<object> InvokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;

            var sessionId = new Guid();

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
