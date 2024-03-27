using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingRecordingsQueryResponse : ServerResponse
    {
        [JsonProperty("recordingFiles")]
        [JsonPropertyName("recordingFiles")]
        public List<MeetingRecordingResponse> RecordingFiles { get; set; }
    }
}
