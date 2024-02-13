using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetEventsResponse : ServerResponse
    {
        [JsonProperty("events"), JsonPropertyName("events")]
        public List<SimpleEvent> Events { get; set; }
    }
}
