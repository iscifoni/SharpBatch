using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpBatch;

namespace WebApplication.Attributes
{
    public class MyExceptionAttribute : BatchExceptionAttribute
    {
        public override void onExecuted(BatchExecutionContext context, Exception ex)
        {
            context.Response.Body.Write(Encoding.UTF8.GetBytes("Error - "), 0, 5);
        }
    }
}
