using MacroPad.Shared.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Shared.Plugin.Nodes.Components
{
    public class ComboBox : INodeComponent
    {
        public Action<IResourceManager, int>? SelectionChanged { get; set; }
        public Func<IResourceManager, int>? GetSelection { get; set; }
        public Func<IResourceManager, string>? GetSelectedItem { get; set; }
        public Func<IResourceManager, IDeviceLayoutButton, IDeviceOutput, string[]>? GetItems { get; set; }
        private string[] _items = [];
        public string[] Items { get => _items; set {
                _items = value;
                ItemsChanged?.Invoke(this, value);
            } }
        public event EventHandler<string[]>? ItemsChanged;
    }
}
