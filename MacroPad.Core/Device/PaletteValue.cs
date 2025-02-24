using MacroPad.Shared.Device;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Device
{
    public class PaletteValue : IPaletteValue
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
        [JsonPropertyName("color")]
        public uint Color { get; set; }
        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }
}
