//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

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
                DateTime.Today.AddDays(-6),
                DateTime.Today.AddDays(-5),
                DateTime.Today.AddDays(-4),
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(-2),
                DateTime.Today.AddDays(-1),
                DateTime.Today
            };

          
            var dataCompleted = (from date in dateList
                                 join completed in completedList on date equals completed.StartDate.Value.Date into Tab1
                                 orderby date 
                                 select new { date = date, count = Tab1.Count() }).ToList();

            var dataNotCompleted = (from date in dateList
                                    join completed in notCompletedList on date equals completed.StartDate.Value.Date into Tab1
                                    orderby date
                                    select new { date = date, count = Tab1.Count() }).ToList();


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
