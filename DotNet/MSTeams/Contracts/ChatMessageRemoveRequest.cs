using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMessageRemoveRequest
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("messageId")]
        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }
    }
}
