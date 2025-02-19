using MacroPad.Shared.Device;
using MacroPad.Shared.Plugin.Nodes;
using MacroPad.Shared.Plugin.Settings;
using System.Collections.ObjectModel;

namespace MacroPad.Shared.Plugin.Components
{
    public class ComboBox : INodeComponent, ISettingsComponent
    {
        public Action<IResourceManager, int>? SelectionChanged { get; set; }
        public Func<IResourceManager, int>? GetSelection { get; set; }
        public Func<IResourceManager, string>? GetSelectedItem { get; set; }
        public Func<IResourceManager, IDeviceLayoutButton, IDeviceOutput, string[]>? GetItems { get; set; }
        public ObservableCollection<string> Items { get; set; } = [];
    }
}
