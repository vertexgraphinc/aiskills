using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetEventsRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }

        [JsonProperty("eventId"), JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonProperty("optionalParams"), JsonPropertyName("optionalParams")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("eventTypes"), JsonPropertyName("eventTypes")]
            public string EventTypes { get; set; }

            [JsonProperty("iCalUid"), JsonPropertyName("iCalUid")]
            public string ICalUid { get; set; }

            [JsonProperty("maxAttendees"), JsonPropertyName("maxAttendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("maxResults"), JsonPropertyName("maxResults")]
            public int? MaxResults { get; set; }

            [JsonProperty("orderBy"), JsonPropertyName("orderBy")]
            public string OrderBy { get; set; }

            [JsonProperty("pageToken"), JsonPropertyName("pageToken")]
            public string PageToken { get; set; }

            [JsonProperty("privateExtendedProperty"), JsonPropertyName("privateExtendedProperty")]
            public string PrivateExtendedProperty { get; set; }

            [JsonProperty("q"), JsonPropertyName("q")]
            public string Q { get; set; }

            [JsonProperty("sharedExtendedProperty"), JsonPropertyName("sharedExtendedProperty")]
            public string SharedExtendedProperty { get; set; }

            [JsonProperty("showDeleted"), JsonPropertyName("showDeleted")]
            public bool? ShowDeleted { get; set; }

            [JsonProperty("showHiddenInvitations"), JsonPropertyName("showHiddenInvitations")]
            public bool? ShowHiddenInvitations { get; set; }

            [JsonProperty("singleEvents"), JsonPropertyName("singleEvents")]
            public bool? SingleEvents { get; set; }

            [JsonProperty("syncToken"), JsonPropertyName("syncToken")]
            public string SyncToken { get; set; }

            [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
            public DateTime? TimeMax { get; set; }

            [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
            public string TimeZone { get; set; }

            [JsonProperty("updatedMin"), JsonPropertyName("updatedMin")]
            public DateTime? UpdatedMin { get; set; }
        }
    }
}
