using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MacroPad.Core.Models.Config
{
    public class ButtonStatus
    {
        [JsonProperty("value")]
        public JToken? Value { get; set; }
    }
}
