using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMessagesQueryRequest
    {
        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("lastModifiedBeginTime")]
        [JsonPropertyName("lastModifiedBeginTime")]
        public string LastModifiedBeginTime { get; set; }

        [JsonProperty("lastModifiedEndTime")]
        [JsonPropertyName("lastModifiedEndTime")]
        public string LastModifiedEndTime { get; set; }
    }
}
