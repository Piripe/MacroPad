using MacroPad.Shared.Device;
using Newtonsoft.Json;

namespace MacroPad.Core.Device
{
    public class DeviceOutput(OutputType type, PaletteValue[] palette, string? cornerRadius, string? image, uint color) : IDeviceOutput
    {
        [JsonProperty("type")]
        public OutputType OutputType { get; set; } = type;
        [JsonProperty("palette")]
        public IPaletteValue[] Palette { get; set; } = palette;
        [JsonProperty("cornerRadius")]
        public string? CornerRadius { get; set; } = cornerRadius;
        [JsonProperty("image")]
        public string? Image { get; set; } = image;
        [JsonProperty("color")]
        public uint Color { get; set; } = color;
    }
}
