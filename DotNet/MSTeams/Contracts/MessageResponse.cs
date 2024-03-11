using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class MessageResponse
    {
        [JsonProperty("groupTopic")]
        [JsonPropertyName("groupTopic")]
        public string GroupTopic { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("created")]
        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonProperty("lastModified")]
        [JsonPropertyName("lastModified")]
        public string LastModified { get; set; }
    }
}
