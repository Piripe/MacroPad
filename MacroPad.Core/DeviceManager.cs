using MacroPad.Core.Device;
using MacroPad.Core.Models;
using MacroPad.Core.Models.Config;
using MacroPad.Core.Models.Plugin;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Protocol;
using System.Diagnostics;
using System.Reflection;

namespace MacroPad.Core
{
    public class DeviceManager
    {
        public static MacroPadConfig Config { get; } = MacroPadConfig.LoadConfig();
        public static HashSet<DeviceLayout> Layouts { get; } = DeviceLayout.LoadLayouts();
        public static HashSet<DeviceCore> ConnectedDevices { get; } = [];
        public static DeviceCore? SelectedDevice { get; set; }

        public static event EventHandler<DeviceDetectedEventArgs>? DeviceDetected;
        public static event EventHandler<DeviceDetectedEventArgs>? DeviceDisconnected;



        public static void Init()
        {
            NodeManager.Init();

            PluginManager.PluginEnabled += PluginManager_PluginEnabled;
            PluginManager.PluginDisabled += PluginManager_PluginDisabled;

            PluginManager.ScanPlugins();
        }

        private static void PluginManager_PluginDisabled(object? sender, IPluginInfos e)
        {
            foreach (IProtocol protocol in e.Protocols)
            {
                protocol.DeviceDetected -= Protocol_DeviceDetected;
                protocol.DeviceDisconnected -= Protocol_DeviceDisconnected;
            }
        }

        private static void PluginManager_PluginEnabled(object? sender, IPluginInfos e)
        {
            foreach (IProtocol protocol in e.Protocols)
            {
                protocol.DeviceDetected += Protocol_DeviceDetected;
                protocol.DeviceDisconnected += Protocol_DeviceDisconnected;

                protocol.Enable();
            }
        }

        private static void Protocol_DeviceDetected(object? sender, DeviceDetectedEventArgs e)
        {
            if (!Config.DevicesProfiles.ContainsKey(e.Device.Id)) Config.DevicesProfiles.Add(e.Device.Id, new List<DeviceProfile>() { { new DeviceProfile() { Name= "Profile"} } });
            Config.DefaultProfile.TryAdd(e.Device.Id, 0);
            if (Config.DevicesProfiles[e.Device.Id].Count <= Config.DefaultProfile[e.Device.Id] || Config.DefaultProfile[e.Device.Id] < 0) Config.DefaultProfile[e.Device.Id] = 0;

            DeviceCore device = new(e.Device);

            AddDevice(device);
        }

        private static void Protocol_DeviceDisconnected(object? sender, DeviceDetectedEventArgs e)
        {
            ConnectedDevices.RemoveWhere(device=>device.ProtocolDevice == e.Device);
            DeviceDisconnected?.Invoke(sender, e);
        }

        public static void EnableDevice(string deviceId)
        {
            DeviceCore? device = ConnectedDevices.FirstOrDefault((x) => x.ProtocolDevice.Id == deviceId);
            if (device != null)
            {
                EnableDevice(device);
            }
        }
        public static void EnableDevice(DeviceCore device)
        {
            device.Connect();
            Config.EnabledDevices.Add(device.ProtocolDevice.Id);
        }
        public static void DisableDevice(string deviceId)
        {
            DeviceCore? device = ConnectedDevices.FirstOrDefault((x) => x.ProtocolDevice.Id == deviceId);
            if (device != null)
            {
                DisableDevice(device);
            }
        }
        public static void DisableDevice(DeviceCore device)
        {
            device.Disconnect();
            Config.EnabledDevices.Remove(device.ProtocolDevice.Id);
        }
        public static void RemoveDevice(DeviceCore device)
        {
            ConnectedDevices.Remove(device);
            DeviceDisconnected?.Invoke(null, new DeviceDetectedEventArgs(device.ProtocolDevice));
        }
        public static void AddDevice(DeviceCore device)
        {
            ConnectedDevices.Add(device);

            if (Config.EnabledDevices.Contains(device.ProtocolDevice.Id))
            {
                device.Connect();
            }
            DeviceDetected?.Invoke(null, new DeviceDetectedEventArgs(device.ProtocolDevice));
        }
    }
}
