using MacroPad.Core.BasePlugin;
using MacroPad.Shared.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Settings
{
    public class PluginRowViewModel
    {
        public IPluginInfos Plugin {  get; set; }
        public PluginRowViewModel(IPluginInfos plugin) { Plugin = plugin; }
        public PluginRowViewModel() { Plugin = new BasePluginInfos(); }
    }
}
