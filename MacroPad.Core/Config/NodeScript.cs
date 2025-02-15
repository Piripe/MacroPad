using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class NodeScript
    {
        [JsonProperty("x")]
        public int StartX { get; set; }
        [JsonProperty("y")]
        public int StartY { get; set; }
        [JsonProperty("links")]
        public Dictionary<int,NodeLinks> NodesLinks { get; set; } = new Dictionary<int, NodeLinks>();
        [JsonProperty("lines")]
        public Dictionary<int, NodeLine> NodeLines { get; set; } = new Dictionary<int, NodeLine>();
        //[JsonProperty("numbers")]
        //public Dictionary<int, NodeConst<decimal>> NumberConsts { get; set; } = new Dictionary<int, NodeConst<decimal>>();
        //[JsonProperty("strings")]
        //public Dictionary<int, NodeConst<string>> StringConsts { get; set; } = new Dictionary<int, NodeConst<string>>();
        //[JsonProperty("bools")]
        //public Dictionary<int, NodeConst<bool>> BoolConsts { get; set; } = new Dictionary<int, NodeConst<bool>>();
    }
}
