using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace Zoom.DTOs
{
    public class ZoomRecordings
    {
        [JsonProperty("account_id")]
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonProperty("host_id")]
        [JsonPropertyName("host_id")]
        public string HostId { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("recording_count")]
        [JsonPropertyName("recording_count")]
        public int RecordingCount { get; set; }

        [JsonProperty("start_time")]
        [JsonPropertyName("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("total_size")]
        [JsonPropertyName("total_size")]
        public int TotalSize { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("recording_play_passcode")]
        [JsonPropertyName("recording_play_passcode")]
        public string RecordingPlayPasscode { get; set; }

        [JsonProperty("recording_files")]
        [JsonPropertyName("recording_files")]
        public List<ZoomRecording> RecordingFiles { get; set; }
    }
}
