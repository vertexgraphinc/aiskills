using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Zoom.DTOs
{
    public class ZoomRecording
    {
        [JsonProperty("download_url")]
        [JsonPropertyName("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("file_path")]
        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        [JsonProperty("file_size")]
        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }

        [JsonProperty("file_type")]
        [JsonPropertyName("file_type")]
        public string FileType { get; set; }

        [JsonProperty("file_extension")]
        [JsonPropertyName("file_extension")]
        public string FileExtension { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("meeting_id")]
        [JsonPropertyName("meeting_id")]
        public string MeetingId { get; set; }

        [JsonProperty("play_url")]
        [JsonPropertyName("play_url")]
        public string PlayUrl { get; set; }

        [JsonProperty("recording_end")]
        [JsonPropertyName("recording_end")]
        public string RecordingEnd { get; set; }

        [JsonProperty("recording_start")]
        [JsonPropertyName("recording_start")]
        public string RecordingStart { get; set; }

        [JsonProperty("recording_type")]
        [JsonPropertyName("recording_type")]
        public string RecordingType { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
