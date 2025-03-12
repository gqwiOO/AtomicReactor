using UnityEngine;

namespace Raccoons.Serialization.Json
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string str)
        {
            return JsonUtility.FromJson<T>(str);
        }

        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

    }
}
