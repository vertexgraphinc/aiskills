using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingRecordingsQueryRequest
    {
        [JsonProperty("topicDescription")]
        [JsonPropertyName("topicDescription")]
        public string TopicDescription { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("fileType")]
        [JsonPropertyName("fileType")]
        public string FileType { get; set; }
    }
}
