using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.Web.Internals
{
    public interface IReportProvider
    {
        decimal ErrorPercentageToNow();
        decimal CompletedPercentageToNow();
        (List<string> labels, List<(string serieName, List<int> data)> series) ChartWeekly();
    }
}
