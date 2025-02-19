using MacroPad.Core.Device;

namespace MacroPad.Controls.Home
{
    public class DeviceViewerButtonPressedEventArgs(DeviceLayoutButton? button)
    {
        public DeviceLayoutButton? Button { get; set; } = button;
    }
}
