using MacroPad.Core.Device;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class DeviceProfile { 
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("buttons")]
        public Dictionary<int, Dictionary<int, ButtonConfig>> ButtonsConfig { get; set; } = new Dictionary<int, Dictionary<int, ButtonConfig>>();
    }
}
