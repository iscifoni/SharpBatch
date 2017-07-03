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
using SharpBatch.Tracking.Abstraction;
using SharpBatch.Tracking.Memory;


namespace SharpBatch.internals
{
    internal class SharpBatchTrackingFactory:ISharpBatchTrackingFactory
    {
        ISharpBatchTracking _sharpBatchTraking;
        public SharpBatchTrackingFactory(IServiceProvider serviceProvider)
        {
            var sharpBatchTrakingService = (ISharpBatchTracking)serviceProvider.GetService(typeof(ISharpBatchTracking));
            if (sharpBatchTrakingService == null)
            {
                _sharpBatchTraking = new TrackingMemory();
            }
            else
            {
                _sharpBatchTraking = sharpBatchTrakingService;
            }
        }

        public ISharpBatchTracking getTrakingProvider()
        {
            return _sharpBatchTraking;
        }
    }
}
