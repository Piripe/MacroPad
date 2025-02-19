namespace MacroPad.Core.Device
{
    public class DeviceCoreOutputSetEventArgs(DeviceLayoutButton button, object value)
    {
        public DeviceLayoutButton Button { get; set; } = button;
        public object Value { get; set; } = value;
    }
}
