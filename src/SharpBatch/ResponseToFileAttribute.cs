using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class ResponseToFileAttribute : BatchExecutionAttribute
    {
        public string FileName { get; set; } = "Export";
        public string FileExention { get; set; } = "exp";
        public PathString Path { get; set; } = "";

        public bool TimeStampTocken { get; set; } = true;

        public bool SessionIdInFileName { get; set; } = true;

        public override int Order { get; set; } = 10000;

        public override void onExecuted(BatchExecutionContext context)
        {
            var encoder = new UTF8Encoding();
            char[] content;
            var response = context.ShareMessage.Get<IResponseObject>();
            if ( typeof(byte[]).GetTypeInfo().IsAssignableFrom(response.Response.GetType()))
            {
                content = encoder.GetChars( (byte[])response.Response);
            }
            else if(typeof(string).GetTypeInfo().IsAssignableFrom(response.Response.GetType()))
            {
                content = ((string)response.Response).ToCharArray();
            }else if (typeof(char[]).GetTypeInfo().IsAssignableFrom(response.Response.GetType()))
            {
                content = (char[])response.Response;
            }
            else
            {
                throw new NotSupportedException("Response type not supported for ResponseToFileAttribute");
            }

            string fullFileName = FileName;
            if (SessionIdInFileName)
            {
                fullFileName += $"-{response.SessionId.ToString()}";
            }

            if (TimeStampTocken)
            {
                fullFileName += $"-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            }

            if (!string.IsNullOrEmpty(FileExention))
            {
                fullFileName += FileExention;
            }

            var logFile = System.IO.File.Create(fullFileName);
            using(var logWriter = new System.IO.StreamWriter(logFile))
            {
                var logTask = logWriter.WriteAsync(content);
                logTask.Wait();
                logWriter.Flush();
            }

        }

        public override void onExecuting(BatchExecutionContext context)
        {
        }
    }
}
