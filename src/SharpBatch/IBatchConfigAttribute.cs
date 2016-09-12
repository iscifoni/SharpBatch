using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch
{
    public interface IBatchConfigAttribute
    {
        Task onExecuting(BatchConfigContext context);

        Task onExecuted(BatchConfigContext context);
    }
}
