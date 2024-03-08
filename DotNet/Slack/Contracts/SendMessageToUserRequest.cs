using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SendMessageToUserRequest
    {
        [JsonProperty("user_fullname"), JsonPropertyName("user_fullname")]
        public string UserFullName { get; set; }

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
