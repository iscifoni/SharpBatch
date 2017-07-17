﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpBatch.Tracking.Abstraction;
using SharpBatch.Tracking.DB.data;

namespace SharpBatch.Tracking.DB
{
    public class TrackingDb : ISharpBatchTracking
    {
        private TrackingContext _trackingContext;
        public TrackingDb(TrackingContext trackingContext)
        {
            _trackingContext = trackingContext;
        }

        public Task AddExAsync(Guid sessionId, Exception ex)
        {
            return Task.Run(() =>
            {
                var tracking = _trackingContext.Trackings.Where(p => p.SessionId == sessionId).Single();
                if (tracking.Ex == null)
                {
                    tracking.Ex = new List<ExceptionModel>();
                }

                tracking.Ex.Add(new ExceptionModel() {
                    Exception = Newtonsoft.Json.JsonConvert.SerializeObject(ex)
                });
            });
        }

        public Task AddMessageAsync(Guid sessionId, string Message)
        {
            return Task.Run(() =>
            {
                var tracking = _trackingContext.Trackings.Where(p => p.SessionId == sessionId).Single();
                if (tracking.Messages == null)
                {
                    tracking.Messages = new List<MessagesModel>();
                }

                tracking.Messages.Add(new MessagesModel()
                {
                    Message = Message
                });
            });
        }

        public List<BatchTrackingModel> GetDataOfBatchName(string batchName)
        {
            var trackings = _trackingContext.Trackings.Where(p => p.BatchName.Equals(batchName, StringComparison.OrdinalIgnoreCase)).ToList();
            return null;
        }

        public List<BatchTrackingModel> GetErrors()
        {
            return GetByStatus(StatusEnum.Error);
        }

        public int GetErrorsCount()
        {
            return GetNumByStatus(StatusEnum.Error);
        }

        public List<BatchTrackingModel> GetRunning()
        {
            return GetByStatus(StatusEnum.Running);
        }

        public int GetRunningCount()
        {
            return GetNumByStatus(StatusEnum.Running);
        }

        public Task<BatchTrackingModel> GetStatusAsync(Guid SessionId)
        {
            return Task.Run(()=> {
                return _trackingContext.Trackings
                        .Where(p => p.SessionId == SessionId)
                        .Select(p => (BatchTrackingModel)p)
                        .Single<BatchTrackingModel>();
            });
        }

        public Task PingAsync(Guid sessionId)
        {
            return Task.Run(() =>
            {
                try
                {
                    var tracking = _trackingContext.Trackings.Where(p => p.SessionId == sessionId).Single();
                    tracking.Pings.Add(new PingsModel()
                    {
                        PingData = DateTime.Now,
                        TrackingId = tracking.TrackingId
                    });

                    _trackingContext.SaveChanges();
                }catch
                {
                    //nop
                }
            });
        }

        public Task StartAsync(string BatchName, Guid sessionId)
        {
            return Task.Run(() =>
            {
                TrackingModel model = new TrackingModel()
                {
                    BatchName = BatchName,
                    MachineName = System.Environment.MachineName,
                    SessionId = sessionId,
                    StartDate = DateTime.Now,
                    State = StatusEnum.Running.ToString()                    
                };
            });
        }

        public Task StopAsync(Guid sessionId)
        {
            return Task.Run(() =>
            {
                try
                {
                    var tracking = _trackingContext.Trackings.Where(p => p.SessionId == sessionId).Single();
                    tracking.EndDate = DateTime.Now;

                    _trackingContext.SaveChanges();
                }
                catch
                {
                    //nop
                }
            });
        }

        private List<BatchTrackingModel> GetByStatus(StatusEnum status)
        {
            return _trackingContext.Trackings
                .Where(p => p.State == status.ToString())
                .Select(p => (BatchTrackingModel)p)
                .ToList<BatchTrackingModel>();
        }

        private int GetNumByStatus(StatusEnum status)
        {
            return _trackingContext.Trackings.Where(p => p.State == status.ToString()).Count();
        }

    }
}
