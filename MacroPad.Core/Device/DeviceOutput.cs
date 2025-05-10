using MacroPad.Shared.Device;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Device
{
    public class DeviceOutput : IDeviceOutput
    {
        [JsonPropertyName("type")]
        public OutputType OutputType { get; set; } = OutputType.Palette;
        [JsonPropertyName("palette")]
        [JsonConverter(typeof(PaletteArrayConverter))]
        public IPaletteValue[] Palette { get; set; } = [];
        [JsonPropertyName("cornerRadius")]
        public string? CornerRadius { get; set; }
        [JsonPropertyName("image")]
        public string? Image { get; set; }
        [JsonPropertyName("color")]
        public uint Color { get; set; }
    }

    file class PaletteArrayConverter : JsonConverter<IPaletteValue[]>
    {
        public override IPaletteValue[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<PaletteValue[]>(ref reader, options);
            return list ?? Array.Empty<IPaletteValue>();
        }

        public override void Write(Utf8JsonWriter writer, IPaletteValue[] value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

}
