using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class CreateEventRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }

        [JsonProperty("optionalParams"), JsonPropertyName("optionalParams")]
        public OptionalParameters OptionalParams { get; set; }

        [JsonProperty("event"), JsonPropertyName("event")]
        public Event Event { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("conferenceDataVersion"), JsonPropertyName("conferenceDataVersion")]
            public int? ConferenceDataVersion { get; set; }

            [JsonProperty("maxAttendees"), JsonPropertyName("maxAttendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("sendUpdates"), JsonPropertyName("sendUpdates")]
            public string SendUpdates { get; set; }

            [JsonProperty("supportsAttachments"), JsonPropertyName("supportsAttachments")]
            public bool? SupportsAttachments { get; set; }
        }
    }
}
