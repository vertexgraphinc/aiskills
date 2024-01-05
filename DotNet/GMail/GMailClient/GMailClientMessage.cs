using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientMessage
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("threadId")]
        [JsonPropertyName("threadId")]
        public string ThreadId { get; set; }

        [JsonProperty("labelIds")]
        [JsonPropertyName("labelIds")]
        public List<string> LabelIds { get; set; }

        [JsonProperty("snippet")]
        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("historyId")]
        [JsonPropertyName("historyId")]
        public string HistoryId { get; set; }

        [JsonProperty("internalDate")]
        [JsonPropertyName("internalDate")]
        public string InternalDate { get; set; }

        [JsonProperty("payload")]
        [JsonPropertyName("payload")]
        public GMailClientMessagePayload Payload { get; set; }

        [JsonProperty("sizeEstimate")]
        [JsonPropertyName("sizeEstimate")]
        public int SizeEstimate { get; set; }

        [JsonProperty("raw")]
        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}