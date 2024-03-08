using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SetUserStatusRequest
    {
        [JsonProperty("statusEmoji"), JsonPropertyName("statusEmoji")]
        public string StatusEmoji { get; set; }

        [JsonProperty("statusExpiration"), JsonPropertyName("statusExpiration")]
        public string StatusExpiration { get; set; }

        [JsonProperty("statusText"), JsonPropertyName("statusText")]
        public string StatusText { get; set; }
    }
}
