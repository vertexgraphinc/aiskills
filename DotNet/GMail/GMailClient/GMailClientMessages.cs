using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.GMailClient
{
    public class GMailClientMessages
    {
        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public List<GMailClientMessage> Messages { get; set; }
        
        [JsonProperty("nextPageToken")]
        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName("resultSizeEstimate")]
        [JsonProperty("resultSizeEstimate")]
        public int ResultSizeEstimate { get; set; }
    }
}
