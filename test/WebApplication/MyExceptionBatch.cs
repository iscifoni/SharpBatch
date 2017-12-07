using System;
using System.Collections.Generic;
using System.Text;
using SharpBatch;
using WebApplication.Attributes;

namespace WebApplication
{
    [MyException()]
    public class MyExceptionBatch
    {
        public string go()
        {
            throw new BatchException();
        }
    }
}
