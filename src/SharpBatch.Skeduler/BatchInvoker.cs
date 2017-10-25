using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpBatch.internals;

namespace SharpBatch.Skeduler
{
    public class BatchInvoker
    {
        public static Task invoke(BatchActionDescriptor batchActionDescriptor, IOptions<SkedulerSettings> options, ILogger<BatchInvoker> logger)
        {
           return Task.Run(async () =>
           {
               var client = new HttpClient();
               var baseAddress = options.Value.BaseUri;//  "http://localhost:8080/batch";
               client = new HttpClient();
               client.DefaultRequestHeaders.Accept.Clear();
               client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               client.BaseAddress = new Uri(baseAddress);

               var response = await client.GetAsync($"/batch/exec/{batchActionDescriptor.BatchName}/{batchActionDescriptor.ActionName}");
               if (!response.IsSuccessStatusCode)
               {
                   logger.LogError($"Error on calling batch : {batchActionDescriptor.BatchName} Action : {batchActionDescriptor.ActionName} on base url: {baseAddress}");
               }
           });

        }
    }
}
