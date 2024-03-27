using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingCreateResponse : ServerResponse
    {
        [JsonProperty("meeting")]
        [JsonPropertyName("meeting")]
        public MeetingResponse Meeting { get; set; }
    }
}
