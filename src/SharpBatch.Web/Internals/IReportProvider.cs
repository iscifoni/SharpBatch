using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.Web.Internals
{
    public interface IReportProvider
    {
        byte ErrorPercentageToNow();
        byte CompletedPercentageToNow();

    }
}
