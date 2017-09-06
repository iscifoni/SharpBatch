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
using System.Text;
using Microsoft.AspNetCore.Http;

namespace SharpBatch
{
    /// <summary>
    /// Base class to implementing batch.
    /// </summary>
    /// <remarks>
    /// You can derive all your batch class from this, on this class you have same propertys of convenience to same information as request and response class, parameters dictionary, configuration dictionary and other
    /// </remarks>
    [Batch()]
    public class BaseBatch
    {
        /// <summary>
        /// The <see cref="ContextInvoker"/> of current execution.
        /// </summary>
        [BatchContext()]
        public ContextInvoker batchContext { get; set; }

        /// <summary>
        /// The <see cref="HttpRequest"/> of current execution.
        /// </summary>
        public HttpRequest request => batchContext.Request;

        /// <summary>
        /// The <see cref="HttpResponse"/>.
        /// </summary>
        public HttpResponse response => batchContext.Response;

        /// <summary>
        /// The <see cref="BatchParameterDictionary"/> of current execution.
        /// </summary>
        public BatchParameterDictionary parameters => batchContext.Parameters;

        /// <summary>
        /// The sessionId of current execution.
        /// </summary>
        public Guid sessionId => batchContext.SessionId;

        /// <summary>
        /// The parent sessionId of current execution.
        /// If it is null this is the main session.
        /// </summary>
        public Guid? parentSessionId => batchContext.ParentSessionId;

        /// <summary>
        /// The <see cref="batchConfigurationDictionary"/> of current execution.
        /// </summary>
        public batchConfigurationDictionary configuration => batchContext.ActionDescriptor.BatchConfiguration;


    }
}
