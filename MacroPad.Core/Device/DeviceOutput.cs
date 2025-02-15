using MacroPad.Shared.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Device
{
    public class DeviceOutput : IDeviceOutput
    {
        [JsonProperty("type")]
        public OutputType OutputType { get; set; } = OutputType.None;
        [JsonProperty("palette")]
        public IPaletteValue[] Palette { get; set; } = new IPaletteValue[0];
        [JsonProperty("cornerRadius")]
        public string? CornerRadius { get; set; }
        [JsonProperty("image")]
        public string? Image { get; set; }
        [JsonProperty("color")]
        public uint Color { get; set; }

        public DeviceOutput(OutputType type, PaletteValue[] palette, string? cornerRadius, string? image, uint color) {
            OutputType = type;
            Palette = palette;
            CornerRadius = cornerRadius;
            Image = image;
            Color = color;
        }
    }
}
