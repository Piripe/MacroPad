using MacroPad.Core.BasePlugin.Conditions;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class ConditionsCategory : INodeCategory
    {
        public string Name => "Conditions";

        public string Id => "Conditions";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [new Condition()];

        public INodeRunner[] Runners => [];
    }
}
