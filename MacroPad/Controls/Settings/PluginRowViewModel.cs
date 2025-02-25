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
            if (_isEnabled) UpdatePluginStates();
            if (plugin.IconType == PluginIconType.Image) Icon = new Bitmap(Path.Combine(Plugin.PluginDirectory!, Plugin.Icon));
        }
        public PluginRowViewModel() { Plugin = new PluginInfos() { Name = "Dummy Plugin"}; }

        private void UpdatePluginStates()
        {
            IsUnloadable = Plugin.IsUnloadable;
            IsReloadable = Plugin.IsReloadable;
            HasSettings = Plugin.Settings?.Length > 0;
        }

        private bool _isEnabled = false;
        public bool IsEnabled { get { return _isEnabled; } set { 
                this.RaiseAndSetIfChanged(ref _isEnabled, value);
                if (value)
                {
                    PluginManager.EnablePlugin(Plugin);
                    UpdatePluginStates();
                }
                else PluginManager.DisablePlugin(Plugin);
            } }

        public Bitmap? Icon { get; set; }

        private bool _isUnloadable = false;
        public bool IsUnloadable { get => _isUnloadable; set => this.RaiseAndSetIfChanged(ref _isUnloadable, value); }
        private bool _isReloadable = false;
        public bool IsReloadable { get => _isReloadable; set => this.RaiseAndSetIfChanged(ref _isReloadable, value); }
        private bool _hasSettings= false;
        public bool HasSettings { get => _hasSettings; set => this.RaiseAndSetIfChanged(ref _hasSettings, value); }

        public void Reload()
        {

        }
    }
}
