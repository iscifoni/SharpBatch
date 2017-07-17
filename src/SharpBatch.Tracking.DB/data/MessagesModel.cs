using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpBatch.Tracking.DB.data
{
    public class MessagesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 MessageId { get; set; }
        public Int64 TrackingId { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Message { get; set; }
    }
}
