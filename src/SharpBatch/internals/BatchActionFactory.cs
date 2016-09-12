using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    internal class BatchActionFactory : IBatchActionFactory
    {
        IBatchActionProvider _actionProvider;
        IBatchActionProvider _systemProvider;

        public BatchActionFactory(BatchActionProvider actionProvider, SystemActionProvider systemActionProvider)
        {
            _actionProvider = actionProvider;
            _systemProvider = systemActionProvider;
        }

        public IBatchActionProvider getProvider(BatchUrlManager urlManager)
        {
            switch ( urlManager.RequestCommand)
            {
                case BatchUrlManagerCommand.Exec:
                    return _actionProvider;
                case BatchUrlManagerCommand.Status:
                    return _systemProvider;
                default:
                    throw new MissingFieldException($"Command {urlManager.RequestCommand.ToString()} not found");
            }
        }
    }
}
