using DynamicData;
using MacroPad.Core;
using MacroPad.ViewModels;
using MacroPad.Views.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace MacroPad.Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() {
            DeviceManager.DeviceDetected += DeviceManager_DeviceDetected;
            DeviceManager.DeviceDisconnected += DeviceManager_DeviceDisconnected;
            DevicesInNav.AddRange(DeviceManager.ConnectedDevices.Select(x => new DeviceNavViewModel(x)));
        }

        private void DeviceManager_DeviceDisconnected(object? sender, Shared.Plugin.Protocol.DeviceDetectedEventArgs e)
        {
            DevicesInNav.Remove(DevicesInNav.First(x=>x.Device.ProtocolDevice == e.Device));
        }

        private void DeviceManager_DeviceDetected(object? sender, Shared.Plugin.Protocol.DeviceDetectedEventArgs e)
        {
            DevicesInNav.Add(new DeviceNavViewModel(DeviceManager.ConnectedDevices.First(x => x.ProtocolDevice == e.Device)));
        }

        public ObservableCollection<DeviceNavViewModel> DevicesInNav { get; set; } = [];
    }
}
