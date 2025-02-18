using MacroPad.Shared.Device;
using System.Collections.ObjectModel;

namespace MacroPad.Shared.Plugin.Nodes.Components
{
    public class ComboBox : INodeComponent
    {
        public Action<IResourceManager, int>? SelectionChanged { get; set; }
        public Func<IResourceManager, int>? GetSelection { get; set; }
        public Func<IResourceManager, string>? GetSelectedItem { get; set; }
        public Func<IResourceManager, IDeviceLayoutButton, IDeviceOutput, string[]>? GetItems { get; set; }
        public ObservableCollection<string> Items { get; set; } = [];
    }
}
