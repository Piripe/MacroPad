using MacroPad.Core.Models;
using MacroPad.Shared.Plugin.Nodes;
using System.Text.Json.Nodes;

namespace MacroPad.Core.Node
{
    public class NodeResourceManager : ResourceManager, INodeResourceManager
    {
        private readonly Func<int, object> _getValue;

        public object GetValue(int index)
        {
            return _getValue(index);
        }

        public NodeResourceManager(Dictionary<string, JsonValue> data, Func<int, object> getValue) : base(data) { 
            _getValue = getValue;
        }
    }
}
