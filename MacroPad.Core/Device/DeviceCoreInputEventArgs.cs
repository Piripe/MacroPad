namespace MacroPad.Core.Device
{
    public class DeviceCoreInputEventArgs(DeviceLayoutButton button, float value, bool isPressed)
    {
        public DeviceLayoutButton Button { get; set; } = button;
        public float Value { get; set; } = value;
        public bool IsPressed { get; set; } = isPressed;
    }
}
