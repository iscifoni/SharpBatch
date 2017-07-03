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

namespace SharpBatch.Tracking.Abstraction
{
    public class BatchTrackingModel
    {
        public string BatchName { get; set; }
        public Guid SessionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<DateTime> Pings { get; set; } = new List<DateTime>();
        public StatusEnum State { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public List<Exception> Ex { get; set; }

        public string MachineName => System.Environment.MachineName;
    }
}
