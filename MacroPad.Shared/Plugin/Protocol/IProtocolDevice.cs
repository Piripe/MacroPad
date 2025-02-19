namespace MacroPad.Shared.Plugin.Protocol
{
    public interface IProtocolDevice
    {
        public string Name { get; }
        public string Id { get; }
        public string Protocol { get; }
        public bool IsConnected { get; }

        public event EventHandler<DeviceInputEventArgs>? DeviceInput;

        public void Connect();
        public void Disconnect();

        public void SetButtonRGB(int button, int color);
        public void SetButtonPalette(int button, int color);
        public void SetButtonImage(int button, object image);
    }
}
