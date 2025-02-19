using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Shared.Plugin
{
    public class NodeType(string name, Color color, Type type, object defaultValue, Func<object, object?>? typeConverter = null, Func<IResourceManager, object?>? load = null, INodeComponent[]? components = null)
    {
        public string Name { get; } = name;
        public Color Color { get; } = color;
        public Type Type { get; } = type;
        public object DefaultValue { get; } = defaultValue;
        public Func<object, object?>? TypeConverter { get; set; } = typeConverter;
        public Func<IResourceManager, object?>? Load { get; set; } = load;
        public INodeComponent[] Components { get; set; } = components ?? [];
    }
}
