using MacroPad.Core;

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
