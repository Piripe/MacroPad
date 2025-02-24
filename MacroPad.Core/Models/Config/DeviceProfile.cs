using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class DeviceProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("buttons")]
        public Dictionary<int, Dictionary<int, ButtonConfig>> ButtonsConfig { get; set; } = [];
    }
}
