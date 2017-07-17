using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharpBatch.Tracking.DB.data
{
    public class ExceptionModel
    {
        [Key]
        public Int64 ExceptionId { get; set; }
        public string Exception { get; set; }

    }
}
