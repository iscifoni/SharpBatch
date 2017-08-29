using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharpBatch.Tracking.Abstraction;
using SharpBatch.Tracking.DB.data;

namespace SharpBatch.Tracking.DB
{
    public class TrackingDb : ISharpBatchTracking
    {
        private TrackingContext _trackingContext;
        private ILogger<TrackingDb> _logger;
        public TrackingDb(TrackingContext trackingContext, ILogger<TrackingDb> logger)
        {
            _trackingContext = trackingContext;
            _logger = logger;
        }

        public Task AddExAsync(Guid sessionId, Exception ex)
        {
            return Task.Run(() =>
            {
                var tracking = _trackingContext.Trackings
                                    .Include(p => p.Ex)
                                    .Include(p => p.Messages)
                                    .Include(p => p.Pings)
                                    .Where(p => p.SessionId == sessionId).Single();
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
                var tracking = _trackingContext.Trackings
                                    .Include(p => p.Ex)
                                    .Include(p => p.Messages)
                                    .Include(p => p.Pings)
                                    .Where(p => p.SessionId == sessionId).Single();
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

        public List<BatchTrackingModel> GetAll()
        {
            return _trackingContext.Trackings
                        .Include(p => p.Ex)
                        .Include(p => p.Messages)
                        .Include(p => p.Pings)
                        .Select(p => (BatchTrackingModel)p)
                        .ToList<BatchTrackingModel>();
        }

        public int GetByStatusCount(StatusEnum status)
        {
            return _trackingContext.Trackings.Where(p => p.State == status.ToString()).Count();
        }

        public List<BatchTrackingModel> GetDataOfBatchName(string batchName)
        {
            var trackings = _trackingContext
                                .Trackings
                                .Include(p => p.Ex)
                                .Include(p => p.Messages)
                                .Include(p => p.Pings)
                                .Where(p => p.BatchName.Equals(batchName, StringComparison.OrdinalIgnoreCase))
                                .Select(m=>(BatchTrackingModel)m)
                                .ToList<BatchTrackingModel>();
            return trackings;
        }
        
        public Task<BatchTrackingModel> GetStatusAsync(Guid SessionId)
        {
            return Task.Run(()=> {
                return _trackingContext.Trackings
                        .Include(p => p.Ex)
                        .Include(p => p.Messages)
                        .Include(p => p.Pings)
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
                    var tracking = _trackingContext.Trackings
                                        .Include(p => p.Ex)
                                        .Include(p => p.Messages)
                                        .Include(p => p.Pings)
                                        .Where(p => p.SessionId == sessionId).Single();

                    if(tracking.Pings == null)
                    {
                        tracking.Pings = new List<PingsModel>();
                    }

                    tracking.Pings.Add(new PingsModel()
                    {
                        PingData = DateTime.Now,
                        TrackingId = tracking.TrackingId
                    });

                    _trackingContext.SaveChanges();
                }catch(Exception ex)
                {
                    _logger.LogError(ex, "PyngAsync error:");
                }
            });
        }

        public Task StartAsync(string BatchName, Guid sessionId)
        {
            return Task.Run(() =>
            {
                try
                {
                    TrackingModel model = new TrackingModel()
                    {
                        BatchName = BatchName,
                        MachineName = System.Environment.MachineName,
                        SessionId = sessionId,
                        StartDate = DateTime.Now,
                        State = StatusEnum.Running.ToString()
                    };

                    _trackingContext.Trackings.Add(model);
                    _trackingContext.SaveChanges();
                }catch(Exception ex)
                {
                    _logger.LogError(ex, "StartAsync error:");
                }
            });
        }

        public Task StopAsync(Guid sessionId)
        {
            return Task.Run(() =>
            {
                try
                {
                    var tracking = _trackingContext.Trackings
                                        .Include(p => p.Ex)
                                        .Include(p => p.Messages)
                                        .Include(p => p.Pings)
                                        .Where(p => p.SessionId == sessionId).Single();
                    tracking.EndDate = DateTime.Now;
                    if (tracking.Ex != null && tracking.Ex.Count() > 0)
                    {
                        tracking.State = StatusEnum.Error.ToString();
                    }
                    else
                    {
                        tracking.State = StatusEnum.Stopped.ToString();
                    }

                    _trackingContext.SaveChanges();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "StopAsync error:");
                }
            });
        }

        public List<BatchTrackingModel> GetByStatus(StatusEnum status)
        {
            return _trackingContext.Trackings
                .Include(p => p.Ex)
                .Include(p => p.Messages)
                .Include(p => p.Pings)
                .Where(p => p.State == status.ToString())
                .Select(p => (BatchTrackingModel)p)
                .ToList<BatchTrackingModel>();
        }

        public BatchTrackingModel GetDataOfSessionId(Guid SessionId)
        {
            var response = _trackingContext.Trackings
                    .Include(p => p.Ex)
                    .Include(p => p.Messages)
                    .Include(p => p.Pings)
                    .Where(p => p.SessionId == SessionId)
                    .Select(m => (BatchTrackingModel)m)
                    .FirstOrDefault();

            return response;
        }

        public List<BatchTrackingModel> LastWeekData()
        {
            return _trackingContext.Trackings
                       .Where(p=>p.StartDate.HasValue && p.StartDate.Value.Date >= DateTime.Today.AddDays(-7))
                       .Include(p => p.Ex)
                       .Include(p => p.Messages)
                       .Include(p => p.Pings)
                       .Select(p => (BatchTrackingModel)p)
                       .ToList<BatchTrackingModel>();
        }
    }
}
