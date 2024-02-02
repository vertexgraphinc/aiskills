using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class RemoveCalendarRequest
    {
        [JsonProperty("calendar_id"), JsonPropertyName("calendar_id")]
        public string CalendarId { get; set; }
    }
}
