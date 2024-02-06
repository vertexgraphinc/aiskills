using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetRecurringEventRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }

        [JsonProperty("eventId"), JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonProperty("optionalParams"), JsonPropertyName("optionalParams")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("maxAttendees"), JsonPropertyName("maxAttendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("maxResults"), JsonPropertyName("maxResults")]
            public int? MaxResults { get; set; }

            [JsonProperty("originalStart"), JsonPropertyName("originalStart")]
            public DateTime? OriginalStart { get; set; }

            [JsonProperty("pageToken"), JsonPropertyName("pageToken")]
            public string PageToken { get; set; }

            [JsonProperty("showDeleted"), JsonPropertyName("showDeleted")]
            public bool? ShowDeleted { get; set; }

            [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
            public DateTime? TimeMax { get; set; }

            [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
            public string TimeZone { get; set; }
        }
    }
}
