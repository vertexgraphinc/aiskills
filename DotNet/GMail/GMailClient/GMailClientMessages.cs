using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientMessages
    {
        [JsonPropertyName("messages")]
        [JsonProperty("messages")]
        public List<GMailClientMessage> Messages { get; set; }

        
        [JsonPropertyName("nextPageToken")]
        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName("resultSizeEstimate")]
        [JsonProperty("resultSizeEstimate")]
        public int ResultSize { get; set; }
    }
}
