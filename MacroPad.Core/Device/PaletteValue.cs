using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
