using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpBatch.Tracking.DB.data
{
    public class PingsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 PingId { get; set; }
        public Int64 TrackingId { get; set; }
        public DateTime PingData { get; set; }
    }
}
