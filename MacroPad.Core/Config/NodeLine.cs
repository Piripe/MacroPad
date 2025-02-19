using Newtonsoft.Json;

namespace MacroPad.Core.Config
{
    public class NodeLine
    {
        [JsonProperty("n")]
        public int Node;
        [JsonProperty("i")]
        public int PointIndex;
    }
}
