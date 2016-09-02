using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class BatchActionFactory : IBatchActionFactory
    {
        IList<BatchActionDescriptor> _batchActions;

        public BatchActionFactory(ApplicationBatchManager manager)
        {
            _batchActions = manager.BatchActions;
        }

        public BatchActionDescriptor Search(string BatchName, string ActionName)
        {
            var batchActionDescriptors = _batchActions.Where(p => 
                p.BatchName.Equals(BatchName, StringComparison.OrdinalIgnoreCase)
                && p.ActionName.Equals(ActionName, StringComparison.OrdinalIgnoreCase)
            );

            if ( batchActionDescriptors.Count() > 1)
            {
                //To do custom exception
                throw new Exception("Too many batch satisfy the search ");
            }

            if ( batchActionDescriptors.Count() == 0 )
            {
                //To do custom exception
                throw new Exception("No batch satisfy the search ");
            }

            return batchActionDescriptors.First();
        }
    }
}
