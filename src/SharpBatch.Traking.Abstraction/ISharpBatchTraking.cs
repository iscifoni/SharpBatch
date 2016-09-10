using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.Traking.Abstraction
{
    public interface ISharpBatchTraking
    {
        Task PingAsync(Guid sessionId);
        Task StartAsync(Guid sessionId);
        Task StopAsync(Guid sessionId);
    }
}
