using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Zoom.DTOs
{
    public class ZoomMeeting
    {
        [JsonProperty("agenda")]
        [JsonPropertyName("agenda")]
        public string Agenda { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonProperty("host_id")]
        [JsonPropertyName("host_id")]
        public string HostId { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("join_url")]
        [JsonPropertyName("join_url")]
        public string JoinUrl { get; set; }

        [JsonProperty("pmi")]
        [JsonPropertyName("pmi")]
        public string Pmi { get; set; }

        [JsonProperty("start_time")]
        [JsonPropertyName("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("timezone")]
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonProperty("uuid")]
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("auto_recording")]
        [JsonPropertyName("auto_recording")]
        public string AutoRecording { get; set; }
    }
}
