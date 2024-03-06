using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SetUserStatusRequest
    {
        [JsonProperty("status_emoji"), JsonPropertyName("status_emoji")]
        public string StatusEmoji { get; set; }

        [JsonProperty("status_expiration"), JsonPropertyName("status_expiration")]
        public string StatusExpiration { get; set; }

        [JsonProperty("status_text"), JsonPropertyName("status_text")]
        public string StatusText { get; set; }
    }
}
