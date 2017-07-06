using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.Tracking.DB
{
    public class TrackingDb : ISharpBatchTracking
    {
        public List<BatchTrackingModel> GetDataOfBatchName(string batchName)
        {
            throw new NotImplementedException();
        }

        public List<BatchTrackingModel> GetErrors()
        {
            throw new NotImplementedException();
        }

        public int GetErrorsCount()
        {
            throw new NotImplementedException();
        }

        public List<BatchTrackingModel> GetRunning()
        {
            throw new NotImplementedException();
        }

        public int GetRunningCount()
        {
            throw new NotImplementedException();
        }

        public Task<BatchTrackingModel> GetStatusAsync(Guid SessionId)
        {
            throw new NotImplementedException();
        }

        public Task PingAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(string BatchName, Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
