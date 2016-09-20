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
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.Traking.Memory
{
    public class TrakingMemory:ISharpBatchTraking
    {
        private Dictionary<Guid, BatchTrakingModel> _traks = new Dictionary<Guid, BatchTrakingModel>();

        public async Task<BatchTrakingModel> GetStatus(Guid sessionId)
        {
            BatchTrakingModel response = await Task.Run(() =>
            {
                return _traks[sessionId];
            });

            return response;
        }

        public async Task PingAsync(Guid sessionId)
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
