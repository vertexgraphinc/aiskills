using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingChatResponse
    {
        [JsonProperty("meetingTopic")]
        [JsonPropertyName("meetingTopic")]
        public string MeetingTopic { get; set; }

        [JsonProperty("meetingStartTime")]
        [JsonPropertyName("meetingStartTime")]
        public string MeetingStartTime { get; set; }

        [JsonProperty("meetingDuration")]
        [JsonPropertyName("meetingDuration")]
        public int MeetingDuration { get; set; }

        [JsonProperty("meetingType")]
        [JsonPropertyName("meetingType")]
        public int MeetingType { get; set; }

        [JsonProperty("downloadUrl")]
        [JsonPropertyName("downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}
