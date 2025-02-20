using MacroPad.Core.BasePlugin.Device;
using MacroPad.Core.Device;
using MacroPad.ViewModels;
using ReactiveUI;

namespace MacroPad.Views
{
    public class MainEditorViewModel : ViewModelBase
    {
        private DeviceCore _device = null!;
        public DeviceCore Device { get => _device; set => this.RaiseAndSetIfChanged(ref _device, value); }

        public MainEditorViewModel(DeviceCore device)
        {
            Device = device;
        }
        public MainEditorViewModel()
        {
            Device = new DeviceCore(new BaseDevice());
        }
    }
}