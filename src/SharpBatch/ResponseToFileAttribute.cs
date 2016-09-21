using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch
{
    public class ResponseToFileAttribute : Attribute, IBatchExecutionAttribute
    {
        public string FileName { get; set; } = "Export.exp";

        public string Path { get; set; } = "";

        public bool TimeStampTocken { get; set; } = true;

        public bool SessionIdInFileName { get; set; } = true;

        public int Order { get; set; } = 10000;

        public void onExecuted(BatchExecutionContext context)
        {
            byte[] content;
            var response = context.ShareMessage.Get<IResponseObject>();
            if ( typeof(byte[]).GetTypeInfo().IsAssignableFrom(response.Response.GetType()))
            {
                content = (byte[])response.Response;
            }
            else if(typeof(string).GetTypeInfo().IsAssignableFrom(response.Response.GetType()))
            {
                var encoder = new UTF8Encoding();
                content = encoder.GetBytes((string)response.Response);
            }else
            {
                throw new NotSupportedException("Response type not supported for ResponseToFileAttribute");
            }

            string fileName= FileName;
            if (SessionIdInFileName)
            {
                FileName += $"-{response.SessionId.ToString()}";
            }

            if (TimeStampTocken)
            {
                FileName += $"-{DateTime.Now.ToString("YYYYMMDDHHmmssfff")}";
            }

        }

        public void onExecuting(BatchExecutionContext context)
        {
        }
    }
}
