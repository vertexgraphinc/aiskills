using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientThread
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("historyId")]
        [JsonPropertyName("historyId")]
        public string HistoryId { get; set; }

        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public List<GMailClientMessage> Messages { get; set; }

    }
}