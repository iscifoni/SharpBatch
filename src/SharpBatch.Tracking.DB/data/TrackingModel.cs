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
            List<Exception> exList = null;
            if (trackingModel.Ex != null)
            {
                exList = trackingModel.Ex.Select(p => Newtonsoft.Json.JsonConvert.DeserializeObject<Exception>(p.Exception)).ToList<Exception>();
            }

            List<string> messageList = null;
            if (trackingModel.Messages != null)
            {
                messageList = trackingModel.Messages.Select(p => p.Message).ToList<string>();
            }

            List<DateTime> pingList = null;
            if (trackingModel.Pings != null)
            {
                pingList = trackingModel.Pings.Select(p => p.PingData).ToList<DateTime>();
            }

            BatchTrackingModel batchTrackingModel = new BatchTrackingModel()
            {
                BatchName = trackingModel.BatchName,
                EndDate = trackingModel.EndDate,
                Ex = exList ,
                Messages = messageList,
                Pings = pingList,
                SessionId = trackingModel.SessionId,
                StartDate = trackingModel.StartDate,
                State = (StatusEnum)Enum.Parse(typeof(StatusEnum), trackingModel.State)
            };

            return batchTrackingModel;
        }

        public static explicit operator TrackingModel(BatchTrackingModel batchTrackingModel)
        {
            List<ExceptionModel> exList = null;
            if (batchTrackingModel.Ex != null)
            {
                exList = batchTrackingModel.Ex.Select(p => new ExceptionModel() { Exception = JsonConvert.SerializeObject(p) }).ToList<ExceptionModel>();
            }

            List<MessagesModel> messageList = null;
            if(batchTrackingModel.Messages != null)
            {
                messageList = batchTrackingModel.Messages.Select(p => new MessagesModel() { Message = p }).ToList<MessagesModel>();
            }

            List<PingsModel> pingList = null;
            if(batchTrackingModel.Pings != null)
            {
                pingList = batchTrackingModel.Pings.Select(p => new PingsModel() { PingData = p }).ToList<PingsModel>();
            }

            TrackingModel trackingModel = new TrackingModel()
            {
                BatchName = batchTrackingModel.BatchName,
                EndDate = batchTrackingModel.EndDate,
                Ex = exList,
                Messages = messageList,
                Pings = pingList,
                SessionId = batchTrackingModel.SessionId,
                StartDate = batchTrackingModel.StartDate,
                State = batchTrackingModel.State.ToString()
            };

            return trackingModel;
        }
    }
}
