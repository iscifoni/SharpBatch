using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Tracking.Abstraction;

namespace SharpBatch.Web.Internals
{
    public class ReportProvider : IReportProvider
    {
        ISharpBatchTracking _sharpBatchTracking = null;

        public ReportProvider(ISharpBatchTracking sharpBatchTracking)
        {
            _sharpBatchTracking = sharpBatchTracking;
        }

        public (List<string> labels, List<(string serieName, List<int> data)> series) ChartWeekly()
        {
            throw new NotImplementedException();
        }

        public decimal CompletedPercentageToNow() => percentage(StatusEnum.Stopped);

        public decimal ErrorPercentageToNow() => percentage(StatusEnum.Error);

        private decimal percentage(StatusEnum status)
        {
            var total = _sharpBatchTracking.GetByStatusCount(StatusEnum.Error) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Running) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Stopped) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Started);
            return ((100 * _sharpBatchTracking.GetByStatusCount(status)) / total);
        }

    }
}
