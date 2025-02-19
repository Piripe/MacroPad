using MacroPad.Core.BasePlugin.Math;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class MathCategory : INodeCategory
    {
        public string Name => "Math";

        public string Id => "Math";
        public Color Color => new Color(40, 40, 40);

        public INodeGetter[] Getters => [new Absolute(), new Floor(), new Map(), new Max(), new Min(), new MinMax(), new Operation(), new Round()];

        public INodeRunner[] Runners => [];
    }
}
