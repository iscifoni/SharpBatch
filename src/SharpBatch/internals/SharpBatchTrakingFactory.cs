using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;
using SharpBatch.Traking.Memory;


namespace SharpBatch.internals
{
    internal class SharpBatchTrakingFactory:ISharpBatchTrakingFactory
    {
        ISharpBatchTraking _sharpBatchTraking;
        public SharpBatchTrakingFactory(IServiceProvider serviceProvider)
        {
            var sharpBatchTrakingService = (ISharpBatchTraking)serviceProvider.GetService(typeof(ISharpBatchTraking));
            if (sharpBatchTrakingService == null)
            {
                _sharpBatchTraking = new TrakingMemory();
            }
            else
            {
                _sharpBatchTraking = sharpBatchTrakingService;
            }
        }

        public ISharpBatchTraking getTrakingProvider()
        {
            return _sharpBatchTraking;
        }
    }
}
