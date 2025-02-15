using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
