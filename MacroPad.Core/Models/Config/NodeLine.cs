using Newtonsoft.Json;

namespace MacroPad.Core.Models.Config
{
    public class NodeLine
    {
        [JsonProperty("n")]
        public int Node;
        [JsonProperty("i")]
        public int PointIndex;
    }
}
