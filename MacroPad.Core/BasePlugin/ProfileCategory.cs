using MacroPad.Core.BasePlugin.Profile;
using MacroPad.Shared.Media;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;

namespace MacroPad.Core.BasePlugin
{
    public class ProfileCategory : INodeCategory
    {
        public string Name => "Profile";

        public string Id => "Profile";
        public Color Color => new(40, 40, 40);

        public INodeGetter[] Getters => [];

        public INodeRunner[] Runners => [new SetProfile()];
    }
}
