using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientThread
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("snippet")]
        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        [JsonPropertyName("historyId")]
        [JsonProperty("historyId")]
        public string HistoryId { get; set; }

        [JsonPropertyName("messages")]
        [JsonProperty("messages")]
        public List<GMailClientMessage> Messages { get; set; }

    }
}
