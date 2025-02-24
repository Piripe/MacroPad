using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class NodeLinks
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("g")]
        public Dictionary<int, int> Getters { get; set; } = [];
        [JsonPropertyName("r")]
        public Dictionary<int, int> Runners { get; set; } = [];
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("d")]
        public Dictionary<string, JsonValue> Data { get; set; } = [];
        [JsonPropertyName("c")]
        public Dictionary<int, Dictionary<string, JsonValue>> Consts { get; set; } = [];
    }
}
