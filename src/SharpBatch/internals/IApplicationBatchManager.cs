using System.Collections.Generic;

namespace SharpBatch.internals
{
    public interface IApplicationBatchManager
    {
        IList<BatchActionDescriptor> BatchActions { get; }

        IEnumerable<BatchActionDescriptor> SearcByNameAndAction(string BatchName, string BatchAction);
    }
}