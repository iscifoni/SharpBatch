﻿//Copyright 2016 Scifoni Ivano
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

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpBatch
{
    /// <summary>
    /// Attribute used to send Batch response to file.
    /// </summary>
    public class ResponseToFileAttribute : BatchExecutionAttribute
    {
        /// <summary>
        /// Filename to create
        /// </summary>
        public string FileName { get; set; } = "Export";

        /// <summary>
        /// Extention file
        /// </summary>
        public string FileExention { get; set; } = "exp";

        /// <summary>
        /// File path
        /// </summary>
        public string Path { get; set; } = "";

        /// <summary>
        /// If true add a time stamp to filename
        /// </summary>
        public bool TimeStampTocken { get; set; } = true;

        /// <summary>
        /// If true add SessionId to filename
        /// </summary>
        public bool SessionIdInFileName { get; set; } = true;

        public override int Order { get; set; } = 10000;

        /// <summary>
        /// Return the filename generated 
        /// </summary>
        public string FullFileName { get; private set; }

        public override void onExecuted(BatchExecutionContext context)
        {
            var encoder = new UTF8Encoding();
            char[] content;
            var response = context.ShareMessage.Get<IResponseObject>();
            var responseType = response.Response.GetType();

            if (typeof(byte[]).GetTypeInfo().IsAssignableFrom(responseType))
            {
                content = encoder.GetChars((byte[])response.Response);
            }
            else
            {
                if (typeof(string).GetTypeInfo().IsAssignableFrom(responseType))
                {
                    content = ((string)response.Response).ToCharArray();
                }
                else
                {
                    if (typeof(char[]).GetTypeInfo().IsAssignableFrom(responseType))
                    {
                        content = (char[])response.Response;
                    }
                    else
                    {
                        throw new NotSupportedException("Response type not supported for ResponseToFileAttribute");
                    }
                }
            }

            FullFileName = $"{Path.ToString()}{FileName}";
            if (SessionIdInFileName)
            {
                FullFileName += $"-{response.SessionId.ToString()}";
            }

            if (TimeStampTocken)
            {
                FullFileName += $"-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            }

            if (!string.IsNullOrEmpty(FileExention))
            {
                FullFileName += $".{FileExention}";
            }

            var logFile = System.IO.File.Create(FullFileName);
            using(var logWriter = new System.IO.StreamWriter(logFile))
            {
                var logTask = logWriter.WriteAsync(content);
                logTask.Wait();
                logWriter.Flush();
            }

        }

        public override void onExecuting(BatchExecutionContext context)
        {
        }
    }
}
