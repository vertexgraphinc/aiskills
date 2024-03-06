using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SendMessageRequest
    {
        [JsonProperty("channel"), JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
