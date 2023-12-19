using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientMessageId
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("threadId")]
        [JsonProperty("threadId")]
        public string ThreadId { get; set; }
    }
}