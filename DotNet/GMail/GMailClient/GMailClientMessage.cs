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

        [JsonProperty("labelIds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("labelIds")]
        public List<string> LabelIds { get; set; }

        [JsonProperty("snippet", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("snippet")]
        public string Snippet { get; set; }

        [JsonProperty("historyId", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("historyId")]
        public string HistoryId { get; set; }

        [JsonProperty("internalDate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("internalDate")]
        public string InternalDate { get; set; }

        [JsonProperty("payload", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("payload")]
        public GMailClientMessagePayload Payload { get; set; }

        [JsonProperty("sizeEstimate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("sizeEstimate")]
        public int SizeEstimate { get; set; }

        [JsonProperty("raw", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("raw")]
        public string Raw { get; set; }
    }
}