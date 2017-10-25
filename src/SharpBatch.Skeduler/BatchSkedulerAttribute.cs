using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.Skeduler
{
    public class BatchSkedulerAttribute : BatchConfigAttribute
    {
        public BatchSkedulerAttribute(string token)
            : base("SkedulerToken", token) 
        {

        }
    }
}
