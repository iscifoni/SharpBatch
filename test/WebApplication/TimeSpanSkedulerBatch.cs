using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch;
using SharpBatch.Skeduler;

namespace WebApplication
{
    [Batch()]
    public class TimeSpanSkedulerBatch
    {
        [BatchSkeduler(seconds:60)]
        public DateTime everyMinute()
        {
            return DateTime.Now;
        }
    }
}
