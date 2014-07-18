using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Services.Data.Serializer
{
    public class JSONSerializer : ISerializer
    {
        public string Serialize<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                byte[] bytes = ms.ToArray();
                string json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                ms.Dispose();
                return json;
            }
        }

        public string SerializeDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var chunks = dictionary.Select((pair) =>
            {
                string value = pair.Value.ToString();
                if (!(pair.Value is int || pair.Value is long))
                {
                    value = "\"" + value.Replace("\"", "\\\"") + "\"";
                }
                return "\"" + pair.Key.ToString().Replace("\"", "\\\"") + "\":" + value;
            });

            return "{" + String.Join(",", chunks) + "}";
        }
        
        public T Deserialize<T>(string data)
        {
            T obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var serializer = new DataContractJsonSerializer(typeof(T));
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

    }
}
