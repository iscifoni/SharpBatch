using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class BatchActionProvider : IBatchActionProvider
    {
        IList<BatchActionDescriptor> _batchActions;
        IBatchInvokerProvider _batchInvokerProvider;

        public BatchActionProvider(ApplicationBatchManager manager,
            IBatchInvokerProvider batchInvokerProvider)
        {
            _batchActions = manager.BatchActions;
            _batchInvokerProvider = batchInvokerProvider;
        }

        public async Task<string> InvokeAsync(BatchUrlManager urlManager, ContextInvoker context)
        {
            var batchActionDescriptor = Search(urlManager.RequestBatchName, urlManager.RequestBatchAction);
            context.ActionDescriptor = batchActionDescriptor;

            var asyncCall = (bool)(batchActionDescriptor.BatchConfiguration["AsynCall"] ?? true);

            if (asyncCall)
            {
                var response = _batchInvokerProvider.InvokeAsync(context);
                return "Batch Started";
            }
            else
            {
                var response = await _batchInvokerProvider.InvokeAsync(context) as string;
                return response.ToString();
            }
        }

        private BatchActionDescriptor Search(string BatchName, string ActionName)
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
