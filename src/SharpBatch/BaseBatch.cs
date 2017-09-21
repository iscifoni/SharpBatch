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
using System.Reflection;
using System.Threading;
using System.Text;
using Microsoft.AspNetCore.Http;
using SharpBatch.internals;
using SharpBatch.Serialization.Abstract;
using SharpBatch.Tracking.Abstraction;
using System.Threading.Tasks;

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
        public ContextInvoker BatchContext { get; set; }

        /// <summary>
        /// The <see cref="HttpRequest"/> of current execution.
        /// </summary>
        public HttpRequest Request => BatchContext.Request;

        /// <summary>
        /// The <see cref="HttpResponse"/>.
        /// </summary>
        public HttpResponse Response => BatchContext.Response;

        /// <summary>
        /// The <see cref="BatchParameterDictionary"/> of current execution.
        /// </summary>
        public BatchParameterDictionary Parameters => BatchContext.Parameters;

        /// <summary>
        /// The sessionId of current execution.
        /// </summary>
        public Guid SessionId => BatchContext.SessionId;

        /// <summary>
        /// The parent sessionId of current execution.
        /// If it is null this is the main session.
        /// </summary>
        public Guid? ParentSessionId => BatchContext.ParentSessionId;

        /// <summary>
        /// The <see cref="batchConfigurationDictionary"/> of current execution.
        /// </summary>
        public batchConfigurationDictionary Configuration => BatchContext.ActionDescriptor.BatchConfiguration;

        /// <summary>
        /// Save the value of content into a file.
        /// </summary>
        /// <param name="content">Data to save into a file</param>
        /// <param name="fileName">The file name to use, if not exist will be created</param>
        /// <param name="fileExtention">The extention of a file</param>
        /// <param name="Path">The path where the file is created</param>
        /// <param name="timeStampToken">If true insert the timestamp into file name</param>
        /// <param name="sessionIdInFileName">If true inset the sessionId into file name</param>
        /// <returns>The file name generated</returns>
        public string ToFile(object content, string fileName, string fileExtention, string Path, bool timeStampToken, bool sessionIdInFileName)
        {
            var encoder = new UTF8Encoding();
            char[] contentToSave;
            var response = new ResponseObject(content, SessionId);
            var responseType = response.Response.GetType();
            string fullFileName;

            if (typeof(byte[]).GetTypeInfo().IsAssignableFrom(responseType))
            {
                contentToSave = encoder.GetChars((byte[])response.Response);
            }
            else
            {
                if (typeof(string).GetTypeInfo().IsAssignableFrom(responseType))
                {
                    contentToSave = ((string)response.Response).ToCharArray();
                }
                else
                {
                    if (typeof(char[]).GetTypeInfo().IsAssignableFrom(responseType))
                    {
                        contentToSave = (char[])response.Response;
                    }
                    else
                    {
                        throw new NotSupportedException("Response type not supported for ResponseToFileAttribute");
                    }
                }
            }

            fullFileName = $"{Path ?? ""}{fileName}";
            if (sessionIdInFileName)
            {
                fullFileName += $"-{response.SessionId.ToString()}";
            }

            if (timeStampToken)
            {
                fullFileName += $"-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            }

            if (!string.IsNullOrEmpty(fileExtention))
            {
                fullFileName += $".{fileExtention}";
            }

            var logFile = System.IO.File.Create(fullFileName);
            using(var logWriter = new System.IO.StreamWriter(logFile))
            {
                var logTask = logWriter.WriteAsync(contentToSave);
                logTask.Wait();
                logWriter.Flush();
            }

            return fullFileName;
        }

        /// <summary>
        /// Save the value of content into a file.
        /// </summary>
        /// <param name="content">Data to save into a file</param>
        /// <param name="fileName">The file name to use, if not exist will be created</param>
        /// <param name="fileExtention">The extention of a file</param>
        /// <param name="Path">The path where the file is created</param>
        /// <param name="timeStampToken">If true insert the timestamp into file name</param>
        /// <returns>The file name generated</returns>
        public string ToFile(object content, string fileName, string fileExtention, string Path, bool timeStampToken)
        {
            return ToFile(content, fileName, fileExtention, Path, timeStampToken, false);
        }

        /// <summary>
        /// Save the value of content into a file.
        /// </summary>
        /// <param name="content">Data to save into a file</param>
        /// <param name="fileName">The file name to use, if not exist will be created</param>
        /// <param name="fileExtention">The extention of a file</param>
        /// <param name="Path">The path where the file is created</param>
        /// <returns>The file name generated</returns>
        public string ToFile(object content, string fileName, string fileExtention, string Path)
        {
            return ToFile(content, fileName, fileExtention, Path, false, false);
        }

        /// <summary>
        /// Save the value of content into a file.
        /// </summary>
        /// <param name="content">Data to save into a file</param>
        /// <param name="fileName">The file name to use, if not exist will be created</param>
        /// <param name="fileExtention">The extention of a file</param>
        /// <returns>The file name generated</returns>
        public string ToFile(object content, string fileName, string fileExtention)
        {
            return ToFile(content, fileName, fileExtention, "", false, false);
        }

        /// <summary>
        /// Save the value of content into a file.
        /// </summary>
        /// <param name="content">Data to save into a file</param>
        /// <param name="fileName">The file name to use, if not exist will be created</param>
        /// <returns>The file name generated</returns>
        public string ToFile(object content, string fileName)
        {
            return ToFile(content, fileName, "", "", false, false);
        }

        /// <summary>
        /// Save the value of content into message section of a <see cref="ISharpBatchTracking"/>
        /// </summary>
        /// <param name="content">The content to save</param>
        /// <returns></returns>
        public Task ToTracking(string content)
        {
            ISharpBatchTrackingFactory trackingFactory = (ISharpBatchTrackingFactory)BatchContext.RequestServices.GetService(typeof(ISharpBatchTrackingFactory));
            ISharpBatchTracking tracking = trackingFactory.getTrakingProvider();

            return tracking.AddMessageAsync(SessionId, content);
        }

        /// <summary>
        /// Save the serialized value of content into message section of a <see cref="ISharpBatchTracking"/>, 
        /// </summary>
        /// <param name="data">The content to save</param>
        /// <returns></returns>
        /// <remarks>The <see cref="IModelSerializer"/> used is returned from registered service </remarks>
        public Task ToTracking(object data)
        {
            IModelSerializer serializer = (IModelSerializer)BatchContext.RequestServices.GetService(typeof(IModelSerializer));
            return ToTracking(serializer.Serialize(data));
        }

        /// <summary>
        /// Save the serialized value of content into message section of a <see cref="ISharpBatchTracking"/>, 
        /// </summary>
        /// <param name="data">The content to save</param>
        /// <param name="serializer">The <see cref="IModelSerializer"/> to use to serialize data.</param>
        /// <returns></returns>
        public Task ToTracking(object data, IModelSerializer serializer)
        {
            return ToTracking(serializer.Serialize(data));
        }
    }
}
