using System;
using Newtonsoft.Json;
using SharpBatch.Serialization.Abstract;

namespace SharpBatch.Serialization.Json
{
    public class ModelJsonSerializer:IModelSerializer
    {

        public T Deserialize<T>(string data)
        {
            return (T)JsonConvert.DeserializeObject(data, typeof(T));
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
