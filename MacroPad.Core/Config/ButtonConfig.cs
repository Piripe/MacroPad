using MacroPad.Core.Device;
using Newtonsoft.Json;

namespace MacroPad.Core.Config
{
    public class ButtonConfig
    {
        [JsonProperty("events")]
        public Dictionary<ButtonEvent, NodeScript> EventScripts { get; set; } = [];
        [JsonProperty("status")]
        public ButtonStatus Status { get; set; } = new ButtonStatus();
    }
}
