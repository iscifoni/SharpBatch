//Copyright 2016 Scifoni Ivano
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpBatch.internals
{
    public class BatchActionFactory : IBatchActionFactory
    {
        IBatchActionProvider _actionProvider;
        IBatchActionProvider _systemProvider;

        public BatchActionFactory(BatchActionProvider actionProvider, SystemActionProvider systemActionProvider)
        {
            if (actionProvider == null)
            {
                throw new ArgumentNullException(nameof(actionProvider));
            }

            if (systemActionProvider == null)
            {
                throw new ArgumentNullException(nameof(systemActionProvider));
            }

            _actionProvider = actionProvider;
            _systemProvider = systemActionProvider;
        }

        public IBatchActionProvider getProvider(IBatchUrlManager urlManager)
        {
            switch ( urlManager.RequestCommand)
            {
                case BatchUrlManagerCommand.Exec:
                    return _actionProvider;
                case BatchUrlManagerCommand.Status:
                    return _systemProvider;
                default:
                    throw new ArgumentNullException($"Command {urlManager.RequestCommand.ToString()} not found");
            }
        }
    }
}
