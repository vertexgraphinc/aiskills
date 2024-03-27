using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingsUpdateRequest
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

        [JsonProperty("updatedAgenda")]
        [JsonPropertyName("updatedAgenda")]
        public string UpdatedAgenda { get; set; }

        [JsonProperty("updatedTopic")]
        [JsonPropertyName("updatedTopic")]
        public string UpdatedTopic { get; set; }

        [JsonProperty("updatedStartTime")]
        [JsonPropertyName("updatedStartTime")]
        public string UpdatedStartTime { get; set; }

        [JsonProperty("updatedDuration")]
        [JsonPropertyName("updatedDuration")]
        public string UpdatedDuration { get; set; }
    }
}
