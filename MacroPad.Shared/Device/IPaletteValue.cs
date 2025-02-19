namespace MacroPad.Shared.Device
{
    public interface IPaletteValue
    {
        public int Value { get; }
        public string Name { get; }
        public uint Color { get; }
        public string? Image { get; }
    }
}
