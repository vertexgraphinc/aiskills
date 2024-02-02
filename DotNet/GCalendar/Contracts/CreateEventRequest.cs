using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class CreateEventRequest
    {
        [JsonProperty("calendar_id"), JsonPropertyName("calendar_id")]
        public string CalendarId { get; set; }

        [JsonProperty("optional_params"), JsonPropertyName("optional_params")]
        public OptionalParameters OptionalParams { get; set; }

        [JsonProperty("event"), JsonPropertyName("event")]
        public Event Event { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("conference_data_version"), JsonPropertyName("conference_data_version")]
            public int? ConferenceDataVersion { get; set; }

            [JsonProperty("max_attendees"), JsonPropertyName("max_attendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("send_updates"), JsonPropertyName("send_updates")]
            public string SendUpdates { get; set; }

            [JsonProperty("supports_attachments"), JsonPropertyName("supports_attachments")]
            public bool? SupportsAttachments { get; set; }
        }
    }
}
