using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingsQueryResponse : ServerResponse
    {
        [JsonProperty("meetings")]
        [JsonPropertyName("meetings")]
        public List<MeetingResponse> Meetings { get; set; }
    }
}
