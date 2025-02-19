using MacroPad.Core.BasePlugin.Text;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class TextCategory : INodeCategory
    {
        public string Name => "Text Management";

        public string Id => "Text";
        public Color Color => new(20, 250, 20);

        public INodeGetter[] Getters => [new Join(), new Replace()];

        public INodeRunner[] Runners => [];
    }
}
