using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingRecordingResponse
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

        [JsonProperty("filePath")]
        [JsonPropertyName("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("fileSize")]
        [JsonPropertyName("fileSize")]
        public int FileSize { get; set; }

        [JsonProperty("fileType")]
        [JsonPropertyName("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileExtension")]
        [JsonPropertyName("fileExtension")]
        public string FileExtension { get; set; }

        [JsonProperty("playUrl")]
        [JsonPropertyName("playUrl")]
        public string PlayUrl { get; set; }

        [JsonProperty("recordingEnd")]
        [JsonPropertyName("recordingEnd")]
        public string RecordingEnd { get; set; }

        [JsonProperty("recordingStart")]
        [JsonPropertyName("recordingStart")]
        public string RecordingStart { get; set; }

        [JsonProperty("recordingType")]
        [JsonPropertyName("recordingType")]
        public string RecordingType { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
