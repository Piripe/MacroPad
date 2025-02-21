using DynamicData;
using MacroPad.Core;
using MacroPad.Core.Device;
using MacroPad.ViewModels;
using MacroPad.Views.Navigation;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MacroPad.Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Dictionary<DeviceCore, MainEditorViewModel> _deviceEditors = [];

        public MainSettingsViewModel Settings { get; } = new();
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

        public MainEditorViewModel? GetDeviceEditor(DeviceCore? device)
        {
            if (device == null) return null;
            if (_deviceEditors.TryGetValue(device, out var editor)) return editor;
            editor = new MainEditorViewModel(device);
            _deviceEditors.Add(device, editor);
            return editor;
        }
    }
}
