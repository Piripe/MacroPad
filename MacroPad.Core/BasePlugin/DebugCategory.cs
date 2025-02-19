using MacroPad.Core.BasePlugin.Debug;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class DebugCategory : INodeCategory
    {
        public string Name => "Debug";

        public string Id => "Debug";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [];

        public INodeRunner[] Runners => [new WriteLine()];
    }
}
