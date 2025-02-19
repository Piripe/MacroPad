using MacroPad.Shared.Device;
using Newtonsoft.Json;

namespace MacroPad.Core.Device
{
    public class PaletteValue : IPaletteValue
    {
        [JsonProperty("value")]
        public int Value { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        [JsonProperty("color")]
        public uint Color { get; set; }
        [JsonProperty("image")]
        public string? Image { get; set; }
    }
}
