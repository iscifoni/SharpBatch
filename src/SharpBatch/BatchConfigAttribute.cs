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
    /// <inherirdoc/>
    public class BatchConfigAttribute : Attribute, IBatchConfigAttributeAsync
    {
        private string _name;
        private object _value;

        /// <summary>
        /// Initialize a new <see cref="BatchConfigAttribute"/>.
        /// </summary>
        /// <param name="name">Config parameter name</param>
        /// <param name="value">Config parameter value</param>
        /// <remarks>If the value parameters is null, the configuration will ignore.</remarks>
        public BatchConfigAttribute(string name, object value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;
            _value = value;
        }

        /// <inheritdoc/>
        public int Order { get; set; } = -15000;

        /// <inheritdoc/>
        public virtual Task onExecuted(BatchConfigContext context)
        {
            return TaskWrapper.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual Task onExecuting(BatchConfigContext context)
        {
            if (_value != null)
            {
                context.BatchConfiguration.AddOrUpdate(_name, _value);
            }
            return TaskWrapper.CompletedTask;
        }
    }
}
