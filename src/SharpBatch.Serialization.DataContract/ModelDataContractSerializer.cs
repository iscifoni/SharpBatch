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
