using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class NodeLinks
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";
        [JsonProperty("g")]
        public Dictionary<int,int> Getters { get; set; } = new Dictionary<int,int>();
        [JsonProperty("r")]
        public Dictionary<int, int> Runners { get; set; } = new Dictionary<int, int>();
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("d")]
        public Dictionary<string, JToken> Data { get; set; } = new Dictionary<string, JToken>();
        [JsonProperty("c")]
        public Dictionary<int, Dictionary<string, JToken>> Consts { get; set; } = new Dictionary<int, Dictionary<string, JToken>>();
    }
}
