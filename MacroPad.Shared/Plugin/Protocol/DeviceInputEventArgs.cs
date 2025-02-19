namespace MacroPad.Shared.Plugin.Protocol
{
    public class DeviceInputEventArgs(int buttonID, float value)
    {
        public int ButtonID { get; } = buttonID;
        public float Value { get; } = value;

        public bool IsPressed => Value == 1;
    }
}
