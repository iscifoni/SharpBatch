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
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SharpBatch.Serialization.Abstract;

namespace SharpBatch.Serialization.Xml
{
    /// <summary>
    /// Class used to serialize model to Xml.
    /// </summary>
    public class ModelXmlSerializer : IModelSerializer
    {
        public ModelXmlSerializer()
        {
                
        }

        public T Deserialize<T>(string data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader textReader = new StringReader(data))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader))
                {
                    return (T)xmlSerializer.Deserialize(xmlReader);
                }
            }
        }

        public string Serialize(object data)
        {
            var xmlSerializer = new XmlSerializer(data.GetType());

            using (StringWriterWithEncoding textWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, data);
                }
                return textWriter.ToString();
            }
        }

        public string Serialize<T>(T data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (StringWriterWithEncoding textWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, data);
                }
                return textWriter.ToString();
            }
        }


        private class StringWriterWithEncoding : StringWriter
        {
            public override Encoding Encoding { get; }

            public StringWriterWithEncoding(Encoding encoding)
            {
                Encoding = encoding;
            }
        }

    }
}
