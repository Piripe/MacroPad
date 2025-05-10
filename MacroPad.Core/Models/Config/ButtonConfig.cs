using MacroPad.Core.Device;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class ButtonConfig
    {
        [JsonPropertyName("events")]
        public Dictionary<ButtonEvent, NodeScript> EventScripts { get; set; } = [];
        [JsonPropertyName("status")]
        public ButtonStatus Status { get; set; } = new ButtonStatus();
    }
}
