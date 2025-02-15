using MacroPad.Shared.Plugin.Nodes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Node
{
    public class NodeResourceManager : IResourceManager
    {
        public Dictionary<string, JToken> Data { get; set; }
        private Func<int, object> _getValue;

        public NodeResourceManager(Func<int, object> getValue, Dictionary<string, JToken> data)
        {
            _getValue = getValue;
            Data = data;
        }

        public object GetValue(int index)
        {
            return _getValue(index);
        }

        public T? GetData<T>(string key)
        {
            if (Data.ContainsKey(key)) return Data[key].Value<T>();
            return default;
        }

        public void SetData(string key, object value)
        {
            if (Data.ContainsKey(key)) Data[key] = JToken.FromObject(value);
            else Data.Add(key, JToken.FromObject(value));
        }
    }
}
