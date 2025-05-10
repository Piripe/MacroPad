using MacroPad.Shared.Device;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Device
{
    public class DeviceLayoutButton : IDeviceLayoutButton
    {
        [JsonPropertyName("id")]
        public int Id {  get; set; }
        [JsonPropertyName("type")]
        public ButtonType Type { get; set; }
        [JsonPropertyName("output")]
        public string Output { get; set; } = "";
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("dx")]
        public int DX { get; set; }
        [JsonPropertyName("dy")]
        public int DY { get; set; }
        [JsonPropertyName("dw")]
        public int DWidth { get; set; }
        [JsonPropertyName("dh")]
        public int DHeight { get; set; }
        [JsonPropertyName("rotation")]
        public int Rotation { get; set; }
    }
}
