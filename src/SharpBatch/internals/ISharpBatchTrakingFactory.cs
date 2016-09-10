using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;

namespace SharpBatch.internals
{
    internal interface ISharpBatchTrakingFactory
    {
        ISharpBatchTraking getTrakingProvider();
    }
}
