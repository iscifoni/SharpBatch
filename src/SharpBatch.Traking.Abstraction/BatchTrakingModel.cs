using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.Traking.Abstraction
{
    public class BatchTrakingModel
    {
        public Guid SessionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<DateTime> Pings { get; set; } = new List<DateTime>();
        public string State { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
