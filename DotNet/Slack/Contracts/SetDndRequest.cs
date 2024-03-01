using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SetDndRequest
    {
        [JsonProperty("num_minutes"), JsonPropertyName("num_minutes")]
        public string NumOfMins { get; set; }
    }
}
