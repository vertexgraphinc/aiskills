using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetRecurringEventRequest
    {
        [JsonProperty("calendar_id"), JsonPropertyName("calendar_id")]
        public string CalendarId { get; set; }

        [JsonProperty("event_id"), JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonProperty("optional_params"), JsonPropertyName("optional_params")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("max_attendees"), JsonPropertyName("max_attendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("max_results"), JsonPropertyName("max_results")]
            public int? MaxResults { get; set; }

            [JsonProperty("original_start"), JsonPropertyName("original_start")]
            public DateTime? OriginalStart { get; set; }

            [JsonProperty("page_token"), JsonPropertyName("page_token")]
            public string PageToken { get; set; }

            [JsonProperty("show_deleted"), JsonPropertyName("show_deleted")]
            public bool? ShowDeleted { get; set; }

            [JsonProperty("time_max"), JsonPropertyName("time_max")]
            public DateTime? TimeMax { get; set; }

            [JsonProperty("time_min"), JsonPropertyName("time_min")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("time_zone"), JsonPropertyName("time_zone")]
            public string TimeZone { get; set; }
        }
    }
}
