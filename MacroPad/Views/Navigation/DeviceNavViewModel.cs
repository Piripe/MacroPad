using MacroPad.Core.Device;
using MacroPad.ViewModels;
using ReactiveUI;

namespace MacroPad.Views.Navigation
{
    public class DeviceNavViewModel(DeviceCore device) : ViewModelBase
    {
        public DeviceCore Device { get; set; } = device;

        private bool _enabled = device.ProtocolDevice.IsConnected;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                this.RaiseAndSetIfChanged(ref _enabled, value);
                if (value) Device.Connect();
                else Device.Disconnect();
            }
        }
    }
}
