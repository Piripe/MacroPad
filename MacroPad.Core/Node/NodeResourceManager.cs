using MacroPad.Shared.Plugin.Nodes;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Node
{
    public class NodeResourceManager(Func<int, object> getValue, Dictionary<string, JToken> data) : IResourceManager
    {
        public Dictionary<string, JToken> Data { get; set; } = data;
        private readonly Func<int, object> _getValue = getValue;

        public object GetValue(int index)
        {
            return _getValue(index);
        }

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
    }
}
