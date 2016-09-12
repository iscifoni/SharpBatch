using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    internal class TrakingNone : ISharpBatchTraking
    {
        private Dictionary<Guid, BatchTrakingModel> _traks = new Dictionary<Guid, BatchTrakingModel>();

        public async Task<BatchTrakingModel> GetStatus(Guid sessionId)
        {
            BatchTrakingModel response = await Task.Run(()=>
            {
                return _traks[sessionId];
            });

            return response;
        }

        public async  Task PingAsync(Guid sessionId)
        {
            await Task.Run(() => Ping(sessionId));
        }

        private void Ping(Guid sessionId)
        {
            BatchTrakingModel traking;
            _traks.TryGetValue(sessionId, out traking);
            traking.Pings.Add(DateTime.Now);
        }

        public async Task StartAsync(Guid sessionId)
        {
            await Task.Run(() => Start(sessionId));
        }

        private void Start(Guid sessionId)
        {
            _traks.Add(sessionId, new BatchTrakingModel()
            {
                SessionId = sessionId,
                StartDate = DateTime.Now
            });
        }

        public async Task StopAsync(Guid sessionId)
        {
            await Task.Run(() => Stop(sessionId));
        }

        private void Stop(Guid sessionId)
        {
            BatchTrakingModel traking;
            _traks.TryGetValue(sessionId, out traking);
            traking.EndDate = DateTime.Now;
        }
    }
}
