using MacroPad.Shared.Plugin;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Models
{
    public class ResourceManager(Dictionary<string, JToken> data, Dictionary<VirtualDataKey, object?>? virtualData = null) : IResourceManager
    {
        public Dictionary<string, JToken> Data { get; set; } = data;

        public Dictionary<VirtualDataKey, object?> VirtualData { get; set; } = virtualData ?? [];

        public T? GetData<T>(string key)
        {
            if (Data.TryGetValue(key, out JToken? value)) return value.Value<T>();
            return default;
        }

        public void SetData(string key, object value)
        {
            if (Data.ContainsKey(key)) Data[key] = JToken.FromObject(value);
            else Data.Add(key, JToken.FromObject(value));
        }
        public T? GetVirtual<T>(VirtualDataKey key)
        {
            if (VirtualData.TryGetValue(key, out object? value) && value is T result) return result;
            return default;
        }
    }
}
