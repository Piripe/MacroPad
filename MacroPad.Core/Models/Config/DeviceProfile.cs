using Newtonsoft.Json;

namespace MacroPad.Core.Models.Config
{
    public class DeviceProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("buttons")]
        public Dictionary<int, Dictionary<int, ButtonConfig>> ButtonsConfig { get; set; } = [];
    }
}
