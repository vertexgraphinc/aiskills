using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientMessageFull : GMailClientMessageId
    {
        [JsonPropertyName("labelIds")]
        [JsonProperty("labelIds")]
        public List<string> LabelIds { get; set; }

        [JsonPropertyName("snippet")]
        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonPropertyName("payload")]
        [JsonProperty("payload")]
        public GMailClientMessagePayload Payload { get; set; }

        [JsonPropertyName("nextPageToken")]
        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName("resultSizeEstimate")]
        [JsonProperty("resultSizeEstimate")]
        public long ResultSizeEstimate { get; set; }
    }
}