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

        public byte CompletedPercentageToNow()
        {
            throw new NotImplementedException();
        }

        public byte ErrorPercentageToNow()
        {
            throw new NotImplementedException();
        }
    }
}
