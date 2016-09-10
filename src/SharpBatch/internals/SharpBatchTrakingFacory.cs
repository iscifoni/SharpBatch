using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpBatch.Traking.Abstraction;


namespace SharpBatch.internals
{
    internal class SharpBatchTrakingFacory:ISharpBatchTrakingFactory
    {
        ISharpBatchTraking _sharpBatchTraking;
        public SharpBatchTrakingFacory(IServiceProvider serviceProvider)
        {
            var sharpBatchTrakingService = (ISharpBatchTraking)serviceProvider.GetService(typeof(ISharpBatchTraking));
            if (sharpBatchTrakingService == null)
            {
                _sharpBatchTraking = new TrakingNone();
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
