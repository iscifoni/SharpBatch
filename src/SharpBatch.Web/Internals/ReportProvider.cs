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

        public (List<string> labels, List<(string serieName, List<int> data)> series)? ChartWeekly()
        {
            var list = _sharpBatchTracking.LastWeekData();

            var completedList = list.Where(p => p.State == StatusEnum.Stopped).ToList();
            var notCompletedList = list.Where(p => p.State != StatusEnum.Stopped).ToList();

            List<DateTime> dateList = new List<DateTime>() {
                DateTime.Today,
                DateTime.Today.AddDays(-6),
                DateTime.Today.AddDays(-5),
                DateTime.Today.AddDays(-4),
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1)
            };

            //var dataCompleted = (from date in dateList
            //                    join completed in completedList on date equals completed.StartDate.Value.Date
            //                    group completed by completed.StartDate.Value.Date into g
            //                    orderby g.First().StartDate
            //                    select new { date = g.First().StartDate, count = g.Count() } ).ToList() ;

            //var dataNotCompleted = (from date in dateList
            //                        join completed in notCompletedList on date equals completed.StartDate.Value.Date
            //                        group new { completed, date } by new { completed.StartDate.Value.Date, date } into g
            //                        orderby g.Key.Date
            //                        select new { date = g.Key.Date, count = g.Count() }).ToList();

            var dataCompleted = (from date in dateList
                                 join completed in completedList on date equals completed.StartDate.Value.Date into Tab1
                                 from Tab2 in Tab1.DefaultIfEmpty()
                                 group Tab2 by date into g
                                 orderby g.Key 
                                 select new { date = g.Key, count = g.Count(p=>p!= null) }).ToList();

            var dataNotCompleted = (from date in dateList
                                    join completed in notCompletedList on date equals completed.StartDate.Value.Date into Tab1
                                    from Tab2 in Tab1.DefaultIfEmpty()
                                    group Tab2 by date into g
                                    orderby g.Key 
                                    select new { date = g.Key, count = g.Count(p => p != null) }).ToList();


            List<(string serieName, List<int> data)> seriesData = new List<(string serieName, List<int> data)>();
            seriesData.Add(("Completed", dataCompleted.Select(p => p.count).ToList<int>()));
            seriesData.Add(("Error/Not Completed", dataNotCompleted.Select(p => p.count).ToList<int>()));
            
            return (dateList.Select(p=>p.ToShortDateString()).ToList<string>(), seriesData );
        }

        public decimal CompletedPercentageToNow() => percentage(StatusEnum.Stopped);

        public decimal ErrorPercentageToNow() => percentage(StatusEnum.Error);

        public decimal RunningPercentageToNow() => percentage(StatusEnum.Running);

        private decimal percentage(StatusEnum status)
        {
            var total = _sharpBatchTracking.GetByStatusCount(StatusEnum.Error) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Running) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Stopped) + _sharpBatchTracking.GetByStatusCount(StatusEnum.Started);
            return ((100 * _sharpBatchTracking.GetByStatusCount(status)) / total);
        }


    }
}
