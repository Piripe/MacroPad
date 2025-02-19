using MacroPad.Core.BasePlugin.Variables;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class VariableCategory : INodeCategory
    {
        public string Name => "Variables";

        public string Id => "Variable";
        public Color Color => new(20, 250, 20);

        public INodeGetter[] Getters => [new Get()];

        public INodeRunner[] Runners => [new Set()];
    }
}
