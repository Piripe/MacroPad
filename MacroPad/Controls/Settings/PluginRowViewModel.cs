using Avalonia.Media.Imaging;
using MacroPad.Core;
using MacroPad.Core.BasePlugin;
using MacroPad.Core.Models.Plugin;
using MacroPad.ViewModels;
using ReactiveUI;
using System.IO;

namespace MacroPad.Controls.Settings
{
    public class PluginRowViewModel : ViewModelBase
    {
        public PluginInfos Plugin {  get; set; }
        public PluginRowViewModel(PluginInfos plugin) { 
            Plugin = plugin; 
            _isEnabled = plugin.Enabled;
            if (plugin.IconType == PluginIconType.Image) Icon = new Bitmap(Path.Combine(Plugin.PluginDirectory!, Plugin.Icon));
        }
        public PluginRowViewModel() { Plugin = new PluginInfos() { Name = "Dummy Plugin"}; }

        private bool _isEnabled = false;
        public bool IsEnabled { get { return _isEnabled; } set { 
                this.RaiseAndSetIfChanged(ref _isEnabled, value); 
                if (value) PluginManager.EnablePlugin(Plugin);
                else PluginManager.DisablePlugin(Plugin);
            } }

        public Bitmap? Icon { get; set; }
    }
}
