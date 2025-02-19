using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Media;

namespace MacroPad.Core.BasePlugin
{
    internal class ConstantsCategory : INodeCategory
    {
        public string Name => "Constants";

        public string Id => "Constants";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [new Constants.Boolean(), new Constants.Number(), new Constants.Text()];

        public INodeRunner[] Runners => [];
    }
}
