using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class MessageResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("chatId")]
        [JsonPropertyName("chatId")]
        public string ChatId { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("lastModified")]
        [JsonPropertyName("lastModified")]
        public string LastModified { get; set; }
    }
}
