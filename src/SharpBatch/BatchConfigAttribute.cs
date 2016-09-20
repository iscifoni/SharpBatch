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
using SharpBatch.internals;

namespace SharpBatch
{
    public class BatchConfigAttribute : Attribute, IBatchConfigAttributeAsync
    {
        private string _name;
        private object _value;

        public BatchConfigAttribute(string name, object value)
        {
            _name = name;
            _value = value;
        }

        public int Order { get; set; } = -15000;

        public virtual Task onExecuted(BatchConfigContext context)
        {
            return TaskWrapper.CompletedTask;
        }

        public virtual Task onExecuting(BatchConfigContext context)
        {
            context.BatchConfiguration.AddOrUpdate(_name, _value);
            return TaskWrapper.CompletedTask;
        }
    }
}
