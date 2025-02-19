using MacroPad.Shared.Device;
using Newtonsoft.Json;

namespace MacroPad.Core.Device
{
    public class DeviceLayoutButton : IDeviceLayoutButton
    {
        [JsonProperty("id")]
        public int Id {  get; set; }
        [JsonProperty("type")]
        public ButtonType Type { get; set; }
        [JsonProperty("output")]
        public string Output { get; set; } = "";
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("dx")]
        public int DX { get; set; }
        [JsonProperty("dy")]
        public int DY { get; set; }
        [JsonProperty("dw")]
        public int DWidth { get; set; }
        [JsonProperty("dh")]
        public int DHeight { get; set; }
        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }
}
