using System;
using System.Collections.Generic;
using System.Text;

namespace SharpBatch.Serialization.Abstract
{
    public interface IModelSerializer
    {
        string Serialize(object data);
        T Deserialize<T>(string data);
        string Serialize<T>(T data);
    }
}
