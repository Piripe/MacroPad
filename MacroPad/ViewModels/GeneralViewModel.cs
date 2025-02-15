using MacroPad.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.ViewModels
{
    public class GeneralViewModel : ViewModelBase
    {
        public PluginLoader PluginLoader { get; set; }
        public DeviceManager DeviceManager { get; set; }
        public GeneralViewModel()
        {
            PluginLoader = new PluginLoader();
            DeviceManager = new DeviceManager();
        }
    }
}
