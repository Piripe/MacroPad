using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Config
{
    public class NodeLinks
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";
        [JsonProperty("g")]
        public Dictionary<int,int> Getters { get; set; } = [];
        [JsonProperty("r")]
        public Dictionary<int, int> Runners { get; set; } = [];
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("d")]
        public Dictionary<string, JToken> Data { get; set; } = [];
        [JsonProperty("c")]
        public Dictionary<int, Dictionary<string, JToken>> Consts { get; set; } = [];
    }
}
