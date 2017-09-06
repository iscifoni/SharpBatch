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
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using SharpBatch.Serialization.Abstract;

namespace SharpBatch.Serialization.DataContract
{
    /// <summary>
    /// Class used to serialize model to Xml DataContract.
    /// </summary>
    public class ModelDataContractSerializer : IModelSerializer
    {
        public T Deserialize<T>(string data)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(data), new XmlDictionaryReaderQuotas()))
            {
                return (T)serializer.ReadObject(reader, true);
            }
        }

        public string Serialize(object data)
        {
            var serializer = new DataContractSerializer(data.GetType());

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, data);
                return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }

        public string Serialize<T>(T data)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, data);
                return Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
    }
}
