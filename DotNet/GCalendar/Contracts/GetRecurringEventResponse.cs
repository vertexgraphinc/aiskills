using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetRecurringEventResponse : ServerResponse
    {
        [JsonProperty("events"), JsonPropertyName("events")]
        public List<Event> Events { get; set; }
    }
}
