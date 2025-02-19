using MacroPad.Shared.Plugin.Protocol;

namespace MacroPad.Core.BasePlugin.Device
{
    public class BaseDevice : IProtocolDevice
    {
        public string Name => "Base Device";

        public string Id => "BaseDevice";

        public string Protocol => "NullProtocol";

        public bool IsConnected => false;

        public event EventHandler<DeviceInputEventArgs>? DeviceInput;

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SetButtonImage(int button, object image)
        {
            throw new NotImplementedException();
        }

        public void SetButtonPalette(int button, int color)
        {
            throw new NotImplementedException();
        }

        public void SetButtonRGB(int button, int color)
        {
            throw new NotImplementedException();
        }
    }
}
