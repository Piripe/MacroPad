using MacroPad.Core.BasePlugin.Branching;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class BranchingCategory : INodeCategory
    {
        public string Name => "Branching";

        public string Id => "Branching";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [new GetBranch()];

        public INodeRunner[] Runners => [new SetPaletteColor()];
    }
}
