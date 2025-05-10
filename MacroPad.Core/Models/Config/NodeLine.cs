using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class NodeLine
    {
        [JsonPropertyName("n")]
        public int Node { get; set; }
        [JsonPropertyName("i")]
        public int PointIndex { get; set; }
    }
}
