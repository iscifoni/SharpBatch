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
