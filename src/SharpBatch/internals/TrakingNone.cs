using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    internal class TrakingNone : ISharpBatchTraking
    {
        public async  Task PingAsync(Guid sessionId)
        {
        }

        public async Task StartAsync(Guid sessionId)
        {
        }

        public async Task StopAsync(Guid sessionId)
        {
        }
    }
}
