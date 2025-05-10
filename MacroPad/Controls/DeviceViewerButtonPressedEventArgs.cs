using MacroPad.Core.Device;

namespace MacroPad.Controls
{
    public class DeviceViewerButtonPressedEventArgs(DeviceLayoutButton? button)
    {
        public DeviceLayoutButton? Button { get; set; } = button;
    }
}
