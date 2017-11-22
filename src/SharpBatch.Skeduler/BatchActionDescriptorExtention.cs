using System;
using System.Collections.Generic;
using System.Text;
using SharpBatch.Crontab;
using SharpBatch.internals;

namespace SharpBatch.Skeduler
{
    public static class BatchActionDescriptorExtention
    {
        public static DateTime GetNextExecutionDate(this BatchActionDescriptor batchActionDescriptor)
        {
            DateTime nextExecutionDate = DateTime.Now.AddSeconds(10);
            var configItem = batchActionDescriptor.BatchConfiguration["NextSkeduledDate"];
            if ( configItem == null)
            {
                batchActionDescriptor.SetNextExecutionDate(nextExecutionDate);
            }else
            {
                nextExecutionDate = (DateTime)((KeyValuePair<string, object>)configItem).Value;
            }

            return nextExecutionDate;
        }

        public static void SetNextExecutionDate(this BatchActionDescriptor batchActionDescriptor, DateTime nextSkeduledDate)
        {
            batchActionDescriptor.BatchConfiguration.AddOrUpdate("NextSkeduledDate", nextSkeduledDate);
        }

        public static void Reskedule(this BatchActionDescriptor batchActionDescriptor, DateTime currentDate)
        {
            var skedulerToken = batchActionDescriptor.BatchConfiguration["SkedulerToken"];
            DateTime nextDate;

            if (skedulerToken == null)
            {
                var skedulerTimeSpan = batchActionDescriptor.BatchConfiguration["SkedulerTimeSpan"];
                nextDate = currentDate.Add((TimeSpan)((KeyValuePair<string, object>)skedulerTimeSpan).Value);
            }
            else
            {
                nextDate = CrontabParser.getNextDateTime((string)((KeyValuePair<string, object>)skedulerToken).Value, currentDate);
            }

            batchActionDescriptor.SetNextExecutionDate(nextDate);

        }

    }
}
