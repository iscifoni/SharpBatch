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
