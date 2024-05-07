using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingResponse
    {
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("createdAt")]
        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonProperty("hostEmail")]
        [JsonPropertyName("hostEmail")]
        public string HostEmail { get; set; }

        [JsonProperty("joinUrl")]
        [JsonPropertyName("joinUrl")]
        public string JoinUrl { get; set; }

        [JsonProperty("startTime")]
        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonProperty("autoRecording")]
        [JsonPropertyName("autoRecording")]
        public string AutoRecording { get; set; }
    }
}
