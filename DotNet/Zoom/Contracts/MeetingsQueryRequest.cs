using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingsQueryRequest
    {
        [JsonProperty("topicAgenda")]
        [JsonPropertyName("topicAgenda")]
        public string TopicAgenda { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }
    }
}
