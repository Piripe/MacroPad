using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class MacroPadConfig
    {
        [JsonProperty("plugins")]
        public Dictionary<string, bool> PluginsConfig { get; set; } = new Dictionary<string, bool>();
        [JsonProperty("enabledDevices")]
        public Dictionary<string, bool> EnabledDevices { get; set; } = new Dictionary<string, bool>();
        [JsonProperty("devicesProfile")]
        public Dictionary<string, int> DefaultProfile { get; set; } = new Dictionary<string, int>();
        [JsonProperty("devices")]
        public Dictionary<string, List<DeviceProfile>> DevicesProfiles { get; set; } = new Dictionary<string, List<DeviceProfile>>();
        [JsonProperty("variables")]
        public Dictionary<string, JToken> Variables { get; set; } = new Dictionary<string, JToken>();



        public static MacroPadConfig LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                MacroPadConfig? config = JsonConvert.DeserializeObject<MacroPadConfig>(File.ReadAllText("config.json"));
                if (config != null ) return config;
            }
            return new MacroPadConfig();
        }
        public void SaveConfig()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(this,Formatting.None));
        }
    }
}
