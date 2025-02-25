using MacroPad.Core.Models.Config;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Models
{
    public class MacroPadConfig
    {
        [JsonPropertyName("plugins")]
        public Dictionary<string, bool> EnabledPlugins { get; set; } = [];
        [JsonPropertyName("enabledDevices")]
        public Dictionary<string, bool> EnabledDevices { get; set; } = [];
        [JsonPropertyName("devicesProfile")]
        public Dictionary<string, int> DefaultProfile { get; set; } = [];
        [JsonPropertyName("devices")]
        public Dictionary<string, List<DeviceProfile>> DevicesProfiles { get; set; } = [];
        [JsonPropertyName("variables")]
        public Dictionary<string, JsonValue> Variables { get; set; } = [];



        public static MacroPadConfig LoadConfig()
        {
            if (File.Exists("config.json"))
            {
                MacroPadConfig? config = JsonSerializer.Deserialize<MacroPadConfig>(File.ReadAllText("config.json"));
                if (config != null) return config;
            }
            return new MacroPadConfig();
        }
        public void SaveConfig()
        {
            File.WriteAllText("config.json", JsonSerializer.Serialize(this));
        }
    }
}
