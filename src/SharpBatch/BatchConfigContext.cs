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
using Microsoft.AspNetCore.Http;
using SharpBatch.internals;


namespace SharpBatch
{
    /// <summary>
    /// Context used on execution of <see cref="IBatchConfigAttributeAsync"/>
    /// </summary>
    public class BatchConfigContext
    {
        /// <summary>
        /// The <see cref="BatchConfigurationDictionary"/>.
        /// </summary>
        public BatchConfigurationDictionary BatchConfiguration { get; set; }
        /// <summary>
        /// The <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        /// The <see cref="BatchActionDescriptor"/>.
        /// </summary>
        public BatchActionDescriptor ActionDescriptor { get; set; }
    }
}
