using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Protocol;

namespace MacroPad.Core
{
    public class DeviceManager
    {
        public static MacroPadConfig Config { get; } = MacroPadConfig.LoadConfig();
        public static List<DeviceLayout> Layouts { get; } = DeviceLayout.LoadLayouts();
        public static List<DeviceCore> ConnectedDevices { get; } = [];
        public static DeviceCore? SelectedDevice { get; set; }

        public static event EventHandler<DeviceDetectedEventArgs>? DeviceDetected;
        public static event EventHandler<DeviceDetectedEventArgs>? DeviceDisconnected;



        public static void Init() {
            PluginLoader.LoadPlugins();

            NodeManager.Init();

            foreach (IProtocol protocol in PluginLoader.protocols)
            {
                protocol.DeviceDetected += Protocol_DeviceDetected;
                protocol.DeviceDisconnected += Protocol_DeviceDisconnected;

                string? protocolName = protocol.Id;
                if (protocolName == null) return;
                Config.PluginsConfig.TryAdd(protocolName, false);
                if (Config.PluginsConfig[protocolName])
                {
                    protocol.Enable();
                }
            }
        }

        private static void Protocol_DeviceDetected(object? sender, DeviceDetectedEventArgs e)
        {
            Console.WriteLine($"Device detected: {e.Device.Name} ({e.Device.Id})");

            Config.EnabledDevices.TryAdd(e.Device.Id, false);
            if (!Config.DevicesProfiles.ContainsKey(e.Device.Id)) Config.DevicesProfiles.Add(e.Device.Id, new List<DeviceProfile>() { { new DeviceProfile() { Name= "Profile"} } });
            Config.DefaultProfile.TryAdd(e.Device.Id, 0);
            if (Config.DevicesProfiles[e.Device.Id].Count <= Config.DefaultProfile[e.Device.Id] || Config.DefaultProfile[e.Device.Id] < 0) Config.DefaultProfile[e.Device.Id] = 0;

            DeviceCore device = new(e.Device);

            ConnectedDevices.Add(device);

            if (Config.EnabledDevices[e.Device.Id])
            {
                device.Connect();
            }
            DeviceDetected?.Invoke(sender, e);
        }

        private static void Protocol_DeviceDisconnected(object? sender, DeviceDetectedEventArgs e)
        {
            Console.WriteLine($"Device disconnected: {e.Device.Name} ({e.Device.Id})");
            ConnectedDevices.RemoveAll(device=>device.ProtocolDevice == e.Device);
            DeviceDisconnected?.Invoke(sender, e);
        }


        public static void EnablePluginFeature(string pluginId)
        {
            EnableProtocol(pluginId);
        }
        public static void DisablePluginFeature(string pluginId)
        {
            DisableProtocol(pluginId);
        }
        public static void EnableProtocol(string pluginId)
        {
            IProtocol? protocol = PluginLoader.protocols.FirstOrDefault((x) => x.Id == pluginId);
            if (protocol != null)
            {
                protocol.Enable();
                Config.PluginsConfig[pluginId] = true;
            }
        }
        public static void DisableProtocol(string pluginId)
        {
            IProtocol? protocol = PluginLoader.protocols.FirstOrDefault((x) => x.Id == pluginId);
            if (protocol != null)
            {
                protocol.Disable();
                Config.PluginsConfig[pluginId] = false;
            }
        }

        public static void EnableDevice(string deviceId)
        {
            DeviceCore? device = ConnectedDevices.Find((x) => x.ProtocolDevice.Id == deviceId);
            if (device != null)
            {
                device.Connect();
                Config.EnabledDevices[deviceId] = true;
            }
        }
        public static void DisableDevice(string deviceId)
        {
            DeviceCore? device = ConnectedDevices.Find((x) => x.ProtocolDevice.Id == deviceId);
            if (device != null)
            {
                device.Disconnect();
                Config.EnabledDevices[deviceId] = false;
            }
        }
    }
}
