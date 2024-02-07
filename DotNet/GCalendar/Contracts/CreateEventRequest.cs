using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class CreateEventRequest
    {
        [JsonProperty("event"), JsonPropertyName("event")]
        public Event Event { get; set; }
    }
}
