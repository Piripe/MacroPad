using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Settings;

namespace MacroPad.Shared.Plugin.Components
{
    public class NumericUpDown : INodeComponent, ISettingsComponent
    {
        public decimal Min { get; set; } = 0;
        public decimal Max { get; set; } = 100;
        public Action<IResourceManager, decimal>? ValueChanged { get; set; }
        public Func<IResourceManager, decimal>? GetValue { get; set; }
    }
}
