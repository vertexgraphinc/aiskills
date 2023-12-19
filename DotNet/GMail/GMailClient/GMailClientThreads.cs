using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.GMailClient
{
    public class GMailClientThreads
    {
        [JsonProperty("threads")]
        [JsonPropertyName("threads")]
        public List<GMailClientThread> Threads { get; set; }
        
        [JsonProperty("nextPageToken")]
        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName("resultSizeEstimate")]
        [JsonProperty("resultSizeEstimate")]
        public int ResultSizeEstimate { get; set; }
    }
}
