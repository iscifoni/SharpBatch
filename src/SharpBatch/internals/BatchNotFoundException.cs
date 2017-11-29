using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.internals
{
    public class BatchNotFoundException:Exception
    {
        public BatchNotFoundException(string BatchName):
            base($"Batch {BatchName} not found")
        {

        }
    }
}
