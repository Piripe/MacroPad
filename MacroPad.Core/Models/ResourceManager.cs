using MacroPad.Shared.Plugin;
using System.Text.Json.Nodes;

namespace MacroPad.Core.Models
{
    public class ResourceManager(Dictionary<string, JsonValue> data, Dictionary<VirtualDataKey, object?>? virtualData = null) : IResourceManager
    {
        public Dictionary<string, JsonValue> Data { get; set; } = data;

        public Dictionary<VirtualDataKey, object?> VirtualData { get; set; } = virtualData ?? [];

        public T? GetData<T>(string key)
        {
            if (Data.TryGetValue(key, out JsonValue? value)) return value.TryGetValue(out T? value2) ? value2 : default;
            return default;
        }

        public void SetData(string key, object value)
        {
            JsonValue? value2 = JsonValue.Create(value);
            if (value2 != null && !Data.TryAdd(key, value2)) Data[key] = value2;
        }
        public T? GetVirtual<T>(VirtualDataKey key)
        {
            if (VirtualData.TryGetValue(key, out object? value) && value is T result) return result;
            return default;
        }
    }
}
