using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public interface IBatchActionDescriptionProvider
    {
        BatchActionDescriptor actionDescription(IList<Type> types);
    }
}
