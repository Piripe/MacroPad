using MacroPad.Core.BasePlugin.Button;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class ButtonCategory : INodeCategory
    {
        public string Name => "Button";

        public string Id => "Button";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [new GetCurrentValue()];

        public INodeRunner[] Runners => [new SetCurrentPaletteColor()];
    }
}
