using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientMessage
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("threadId")]
        [JsonProperty("threadId")]
        public string ThreadId { get; set; }

        [JsonPropertyName("snippet")]
        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonPropertyName("payload")]
        [JsonProperty("payload")]
        public GMailClientMessagePayload Payload { get; set; }

    }
}
