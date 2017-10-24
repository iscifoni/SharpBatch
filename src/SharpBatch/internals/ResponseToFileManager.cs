using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharpBatch.internals
{
    public class ResponseToFileManager
    {
        private Guid _sessionId;
        public ResponseToFileManager(Guid sessionId)
        {
            _sessionId = sessionId;
        }

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
            var response = new ResponseObject(content, _sessionId);
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
            using (var logWriter = new System.IO.StreamWriter(logFile))
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
        public string ToFile(object content,  string fileName)
        {
            return ToFile(content, fileName, "", "", false, false);
        }

    }
}
