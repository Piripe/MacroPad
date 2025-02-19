using Newtonsoft.Json;

namespace MacroPad.Core.Config
{
    public class NodeScript
    {
        [JsonProperty("x")]
        public int StartX { get; set; }
        [JsonProperty("y")]
        public int StartY { get; set; }
        [JsonProperty("links")]
        public Dictionary<int,NodeLinks> NodesLinks { get; set; } = [];
        [JsonProperty("lines")]
        public Dictionary<int, NodeLine> NodeLines { get; set; } = [];
    }
}
