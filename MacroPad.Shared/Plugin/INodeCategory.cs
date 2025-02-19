using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Shared.Plugin
{
    public interface INodeCategory
    {
        public string Name { get; }
        public string Id { get; }
        public Color Color { get; }

        public INodeGetter[] Getters { get; }
        public INodeRunner[] Runners { get; }
    }
}
