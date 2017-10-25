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
            var configItem = batchActionDescriptor.BatchConfiguration["NextSkeduledDate"];
            if ( configItem == null)
            {
                return DateTime.Now;
            }

            return (DateTime)((KeyValuePair<string, object>)configItem).Value;
        }

        public static void SetNextExecutionDate(this BatchActionDescriptor batchActionDescriptor, DateTime nextSkeduledDate)
        {
            batchActionDescriptor.BatchConfiguration.AddOrUpdate("NextSkeduledDate", nextSkeduledDate);
        }

        public static void Reskedule(this BatchActionDescriptor batchActionDescriptor)
        {
            var skedulerToken = (string)((KeyValuePair<string, object>)batchActionDescriptor.BatchConfiguration["SkedulerToken"]).Value;
            batchActionDescriptor.SetNextExecutionDate(CrontabParser.getNextDateTime(skedulerToken, DateTime.Now));
        }

    }
}
