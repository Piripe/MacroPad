using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Core.Config
{
    public class ButtonStatus
    {
        [JsonProperty("value")]
        public JToken? Value { get; set; }
    }
}
