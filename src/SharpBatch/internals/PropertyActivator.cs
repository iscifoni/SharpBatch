using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SharpBatch.internals
{
    public class PropertyActivator
    {
        private readonly Func<Type, ObjectFactory> _createFactory =
            (type) => ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);

        public TInstance CreateInstance<TInstance>(
           IServiceProvider serviceProvider,
           Type implementationType)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var createFactory = _createFactory(implementationType);
            return (TInstance)createFactory(serviceProvider, arguments: null);
        }
    }
}
