using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.Traking.Memory
{
    public class TrakingMemory:ISharpBatchTraking
    {
        public Task<BatchTrakingModel> GetStatus(Guid SessionId)
        {
            throw new NotImplementedException();
        }

        public async Task PingAsync(Guid sessionId)
        {
            //throw new NotImplementedException();
        }

        public async Task StartAsync(Guid sessionId)
        {
            //throw new NotImplementedException();
        }

        public async Task StopAsync(Guid sessionId)
        {
            //throw new NotImplementedException();
        }
    }
}
