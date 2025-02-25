using MacroPad.Controls.Settings;
using MacroPad.Core;
using MacroPad.Core.Plugin;
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
            Plugins = [.. PluginManager.Plugins.Select(x => new PluginRowViewModel(x))];
            PluginManager.PluginAdded += (s, e) => Plugins.Add(new PluginRowViewModel(e));
            PluginManager.PluginRemoved += (s, e) => Plugins.Remove(Plugins.First(x => x.Plugin == e));
        }
    }
}
