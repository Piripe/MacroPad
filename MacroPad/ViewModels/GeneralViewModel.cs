using MacroPad.Core;

namespace MacroPad.ViewModels
{
    public class GeneralViewModel2 : ViewModelBase
    {
        public PluginLoader PluginLoader { get; set; }
        public DeviceManager DeviceManager { get; set; }
        public GeneralViewModel2()
        {
            PluginLoader = new PluginLoader();
            DeviceManager = new DeviceManager();
        }
    }
}
