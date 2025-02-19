using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Config
{
    public class MacroPadConfig
    {
        [JsonProperty("plugins")]
        public Dictionary<string, bool> PluginsConfig { get; set; } = [];
        [JsonProperty("enabledDevices")]
        public Dictionary<string, bool> EnabledDevices { get; set; } = [];
        [JsonProperty("devicesProfile")]
        public Dictionary<string, int> DefaultProfile { get; set; } = [];
        [JsonProperty("devices")]
        public Dictionary<string, List<DeviceProfile>> DevicesProfiles { get; set; } = [];
        [JsonProperty("variables")]
        public Dictionary<string, JToken> Variables { get; set; } = [];



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
