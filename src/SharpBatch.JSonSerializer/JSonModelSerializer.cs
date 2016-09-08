using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SharpBatch.JSonSerializer
{
    //ToDo optimize it using jsonoption on configuration
    public class JSonModelSerializer
    {
        
        public static T Deserialize<T>(string data)
        {
            return (T)JsonConvert.DeserializeObject(data, typeof(T));
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
