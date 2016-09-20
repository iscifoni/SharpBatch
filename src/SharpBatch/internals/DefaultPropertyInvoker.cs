using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class DefaultPropertyInvoker : IPropertyInvoker
    {
        PropertyActivator _activator;

        public DefaultPropertyInvoker(PropertyActivator activator)
        {
            _activator = activator;
        }
        public Task invokeAsync(ContextInvoker context)
        {
            var actionToExecute = context.ActionDescriptor;

            //Search contextAttribute to set

            //var propertyInfo = actionToExecute.getp ;

            //var executor = PropertyExecutor.Create(actionToExecute.ActionInfo, actionToExecute.BatchTypeInfo);
            //var activatorInstance = _activator.CreateInstance<object>(context.RequestServices, actionToExecute.BatchTypeInfo.AsType());
            //var result = executor.Execute(activatorInstance, parameters);


            return TaskWrapper.CompletedTask;
        }
    }
}
