using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.Tracking.DB.data
{
    public class TrackingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 TrackingId { get; set; }
        public string BatchName { get; set; }
        public Guid SessionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PingsModel> Pings { get; set; } 
        public string State { get; set; }
        public List<MessagesModel> Messages { get; set; }
        public List<ExceptionModel> Ex { get; set; }

        public string MachineName { get; set; }

        public static explicit operator BatchTrackingModel(TrackingModel trackingModel)
        {
            BatchTrackingModel batchTrackingModel = new BatchTrackingModel()
            {
                BatchName = trackingModel.BatchName,
                EndDate = trackingModel.EndDate,
                Ex = trackingModel.Ex.Select(p => Newtonsoft.Json.JsonConvert.DeserializeObject<Exception>(p.Exception)).ToList<Exception>(),
                Messages = trackingModel.Messages.Select(p => p.Message).ToList<string>(),
                Pings = trackingModel.Pings.Select(p => p.PingData).ToList<DateTime>(),
                SessionId = trackingModel.SessionId,
                StartDate = trackingModel.StartDate,
                State = (StatusEnum)Enum.Parse(typeof(StatusEnum), trackingModel.State)
            };

            return batchTrackingModel;
        }

        public static explicit operator TrackingModel(BatchTrackingModel batchTrackingModel)
        {
            TrackingModel trackingModel = new TrackingModel()
            {
                BatchName = batchTrackingModel.BatchName,
                EndDate = batchTrackingModel.EndDate,
                Ex = batchTrackingModel.Ex.Select(p => new ExceptionModel() { Exception = JsonConvert.SerializeObject(p) }).ToList<ExceptionModel>(),
                Messages = batchTrackingModel.Messages.Select(p => new MessagesModel() { Message = p }).ToList<MessagesModel>(),
                Pings = batchTrackingModel.Pings.Select(p => new PingsModel() { PingData = p }).ToList<PingsModel>(),
                SessionId = batchTrackingModel.SessionId,
                StartDate = batchTrackingModel.StartDate,
                State = batchTrackingModel.State.ToString()
            };

            return trackingModel;
        }
    }
}
