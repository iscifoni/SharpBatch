using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.Skeduler;

namespace WebApplication
{
    [Batch()]
    
    public class SkeduledBatch
    {
        [BatchSkeduler("* * * * *")]
        public string EveryMinute()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}
