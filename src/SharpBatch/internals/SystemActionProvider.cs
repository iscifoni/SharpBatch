using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    internal class SystemActionProvider : IBatchActionProvider
    {
        private ISharpBatchTraking _trakingProvider;
        public SystemActionProvider(ISharpBatchTrakingFactory trakingFactory)
        {
            _trakingProvider = trakingFactory.getTrakingProvider();
        }

        public async Task<string> InvokeAsync(BatchUrlManager urlManager, ContextInvoker context)
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
