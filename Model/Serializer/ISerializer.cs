using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.Serializer
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);
        string SerializeDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary);
        T Deserialize<T>(string data);
    }
}
