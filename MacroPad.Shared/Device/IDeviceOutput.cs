namespace MacroPad.Shared.Device
{
    public interface IDeviceOutput
    {
        public OutputType OutputType { get; }
        public IPaletteValue[] Palette { get; }
        public string? CornerRadius { get; }
        public string? Image { get; }
        public uint Color { get; }
    }
}
