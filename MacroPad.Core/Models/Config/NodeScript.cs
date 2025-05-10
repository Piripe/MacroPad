using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class NodeScript
    {
        [JsonPropertyName("x")]
        public int StartX { get; set; }
        [JsonPropertyName("y")]
        public int StartY { get; set; }
        [JsonPropertyName("links")]
        public Dictionary<int, NodeLinks> NodesLinks { get; set; } = [];
        [JsonPropertyName("lines")]
        public Dictionary<int, NodeLine> NodeLines { get; set; } = [];
    }
}
