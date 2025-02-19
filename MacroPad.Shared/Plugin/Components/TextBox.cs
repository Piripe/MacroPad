using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Settings;

namespace MacroPad.Shared.Plugin.Components
{
    public class TextBox : INodeComponent, ISettingsComponent
    {
        public Action<IResourceManager, string>? TextChanged { get; set; }
        public Func<IResourceManager, string>? GetText { get; set; }
    }
}
