using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MacroPad.Core.Models.Config
{
    public class ButtonStatus
    {
        [JsonPropertyName("value")]
        public JsonValue? Value { get; set; }
    }
}
