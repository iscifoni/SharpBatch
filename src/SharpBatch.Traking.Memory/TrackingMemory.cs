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
using System.Linq.Expressions;
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

        public Task PingAsync(Guid sessionId) => Task.Run(() => Ping(sessionId));

        private void Ping(Guid sessionId)
        {
            _traks.TryGetValue(sessionId, out BatchTrackingModel traking);
            traking.Pings.Add(DateTime.Now);
            traking.State = StatusEnum.Running;
        }

        public Task StartAsync(string BatchName, Guid sessionId) => Task.Run(() => Start(BatchName, sessionId));

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

        public Task StopAsync(Guid sessionId) => Task.Run(() => Stop(sessionId));

        private void Stop(Guid sessionId)
        {
            _traks.TryGetValue(sessionId, out BatchTrackingModel traking);
            traking.EndDate = DateTime.Now;
            traking.State = StatusEnum.Stopped;
        }

        public List<BatchTrackingModel> GetDataOfBatchName(string batchName)
        {
            return _traks
                .Where(p => p.Value.BatchName.Equals(batchName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(o => o.Value.StartDate)
                .Select(m => m.Value)
                .ToList<BatchTrackingModel>();
        }

        public Task AddExAsync(Guid sessionId, Exception ex) => Task.Run(() => AddEx(sessionId, ex));

        private void AddEx(Guid sessionId, Exception ex)
        {
            _traks.TryGetValue(sessionId, out BatchTrackingModel traking);
            if ( traking.Ex == null)
            {
                traking.Ex = new List<Exception>();
            }

            traking.Ex.Add(ex);
        }

        public Task AddMessageAsync(Guid sessionId, string Message) => Task.Run(() => AddMessage(sessionId, Message));

        private void AddMessage(Guid sessionId, string Message)
        {
            _traks.TryGetValue(sessionId, out BatchTrackingModel traking);
            if (traking.Messages == null)
            {
                traking.Messages = new List<string>();
            }

            traking.Messages.Add(Message);
        }

        public List<BatchTrackingModel> GetAll()
        {
            return _traks
                .OrderByDescending(o => o.Value.StartDate)
                .Select(m => m.Value)
                .ToList<BatchTrackingModel>();
        }

        public List<BatchTrackingModel> GetByStatus(StatusEnum status)
        {
            return _traks.Where(p => p.Value.State == status)
                    .OrderByDescending(o => o.Value.StartDate)
                    .Select(m => m.Value)
                    .ToList<BatchTrackingModel>();
        }

        public int GetByStatusCount(StatusEnum status) => _traks.Count(p => p.Value.State == status);

        public BatchTrackingModel GetDataOfSessionId(Guid SessionId)
        {
            return _traks.Where(p => p.Value.SessionId == SessionId)
                  .Select(m => m.Value)
                  .FirstOrDefault();
        }

        public List<BatchTrackingModel> LastWeekData()
        {
            throw new NotImplementedException();
        }
    }
}
