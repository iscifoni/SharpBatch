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
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.Tracking.Memory
{
    public class TrackingMemory:ISharpBatchTracking
    {
        private Dictionary<Guid, BatchTrackingModel> _traks = new Dictionary<Guid, BatchTrackingModel>();

        public Task<BatchTrackingModel> GetStatusAsync(Guid sessionId)
        {
            return Task.Run(() =>
             {
                 return _traks[sessionId];
             });
        }

        public Task PingAsync(Guid sessionId)
        {
            return Task.Run(() => Ping(sessionId));
        }

        private void Ping(Guid sessionId)
        {
            BatchTrackingModel traking;
            _traks.TryGetValue(sessionId, out traking);
            traking.Pings.Add(DateTime.Now);
            traking.State = StatusEnum.Running;
        }

        public Task StartAsync(string BatchName, Guid sessionId)
        {
            return Task.Run(() => Start(BatchName, sessionId));
        }

        private void Start(string batchName, Guid sessionId)
        {
            _traks.Add(sessionId, new BatchTrackingModel()
            {
                BatchName = batchName,
                SessionId = sessionId,
                StartDate = DateTime.Now,
                State = StatusEnum.Started
            });
        }

        public Task StopAsync(Guid sessionId)
        {
            return Task.Run(() => Stop(sessionId));
        }

        private void Stop(Guid sessionId)
        {
            BatchTrackingModel traking;
            _traks.TryGetValue(sessionId, out traking);
            traking.EndDate = DateTime.Now;
            traking.State = StatusEnum.Stopped;
        }

        public List<BatchTrackingModel> GetRunning()
        {
            return _traks
                .Where(p => p.Value.State == StatusEnum.Running)
                .OrderByDescending(o => o.Value.StartDate)
                .Select(m => m.Value)
                .ToList<BatchTrackingModel>();
        }

        public List<BatchTrackingModel> GetErrors()
        {
            return _traks
                .Where(p=>p.Value.State == StatusEnum.Error)
                .OrderByDescending(o=>o.Value.StartDate)
                .Select(m=>m.Value)
                .ToList<BatchTrackingModel>() ;
        }

        public List<BatchTrackingModel> GetDataOfBatchName(string batchName)
        {
            return _traks
                .Where(p => p.Value.BatchName.Equals(batchName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(o => o.Value.StartDate)
                .Select(m => m.Value)
                .ToList<BatchTrackingModel>();
        }

    }
}
