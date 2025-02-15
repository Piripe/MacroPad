using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Shared.Plugin
{
    public class NodeType
    {
        public string Name { get; }
        public Color Color { get; }
        public Type Type { get; }
        public object DefaultValue { get; }
        public Func<object, object?>? TypeConverter { get; set; }
        public Func<IResourceManager, object?>? Load { get; set; }
        public INodeComponent[] Components { get; set; }

        public NodeType(string name, Color color, Type type, object defaultValue, Func<object, object?>? typeConverter = null, Func<IResourceManager, object?>? load = null, INodeComponent[]? components = null)
        {
            Name = name;
            Color = color;
            Type = type;
            DefaultValue = defaultValue;
            TypeConverter = typeConverter;
            Load = load;
            Components = components ?? new INodeComponent[0];
        }
    }
}
