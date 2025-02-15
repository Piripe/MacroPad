using MacroPad.Core.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class ButtonConfig
    {
        [JsonProperty("events")]
        public Dictionary<ButtonEvent, NodeScript> EventScripts { get; set; } = new Dictionary<ButtonEvent, NodeScript>();
        [JsonProperty("status")]
        public ButtonStatus Status { get; set; } = new ButtonStatus();
    }
}
