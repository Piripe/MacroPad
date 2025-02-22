using MacroPad.Controls.Settings;
using MacroPad.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Views.Settings
{
    public class PluginsViewModel
    {
        public ObservableCollection<PluginRowViewModel> Plugins { get; set; }
        public PluginsViewModel() {
            Plugins = [.. PluginLoader.plugins.Select(x => new PluginRowViewModel(x))];
            PluginLoader.PluginAdded += (s, e) => Plugins.Add(new PluginRowViewModel(e));
            PluginLoader.PluginRemoved += (s, e) => Plugins.Remove(Plugins.First(x => x.Plugin == e));
        }
    }
}
