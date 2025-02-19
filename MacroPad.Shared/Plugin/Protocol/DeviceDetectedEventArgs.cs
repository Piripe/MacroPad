namespace MacroPad.Shared.Plugin.Protocol
{
    public class DeviceDetectedEventArgs(IProtocolDevice device)
    {
        public IProtocolDevice Device { get; } = device;
    }
}
