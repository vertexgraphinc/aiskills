using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class RemoveCalendarRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }
    }
}
