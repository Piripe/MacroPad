using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Config
{
    public class ButtonStatus
    {
        [JsonProperty("value")]
        public JToken? Value { get; set; }
    }
}
