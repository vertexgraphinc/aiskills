using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class ClearCalendarRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }
    }
}
