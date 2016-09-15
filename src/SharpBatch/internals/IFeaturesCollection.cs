using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public interface IFeaturesCollection : IEnumerable<KeyValuePair<Type, object>>
    {
        object this[Type key] { get; set; }

        TFeature Get<TFeature>();

        void Set<TFeature>(TFeature instance);
    }
}
