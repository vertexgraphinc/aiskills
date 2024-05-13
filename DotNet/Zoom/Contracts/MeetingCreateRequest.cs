using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingCreateRequest
    {
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("startTime")]
        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonProperty("memberEmails")]
        [JsonPropertyName("memberEmails")]
        public string MemberEmails { get; set; }

        [JsonProperty("autoRecording")]
        [JsonPropertyName("autoRecording")]
        public string AutoRecording { get; set; }
    }
}
