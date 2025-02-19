using MacroPad.Shared.Plugin.Protocol;

namespace MacroPad.Shared.Plugin
{
    public interface IProtocol
    {
        public string Name { get; }
        public string Id { get; }
        public event EventHandler<DeviceDetectedEventArgs>? DeviceDetected;
        public event EventHandler<DeviceDetectedEventArgs>? DeviceDisconnected;
        public void Enable();
        public void Disable();
    }
}
