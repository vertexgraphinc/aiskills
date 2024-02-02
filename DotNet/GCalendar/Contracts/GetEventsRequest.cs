using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetEventsRequest
    {
        [JsonProperty("calendar_id"), JsonPropertyName("calendar_id")]
        public string CalendarId { get; set; }

        [JsonProperty("event_id"), JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonProperty("optional_params"), JsonPropertyName("optional_params")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("event_types"), JsonPropertyName("event_types")]
            public string EventTypes { get; set; }

            [JsonProperty("i_cal_uid"), JsonPropertyName("i_cal_uid")]
            public string ICalUid { get; set; }

            [JsonProperty("max_attendees"), JsonPropertyName("max_attendees")]
            public int? MaxAttendees { get; set; }

            [JsonProperty("max_results"), JsonPropertyName("max_results")]
            public int? MaxResults { get; set; }

            [JsonProperty("order_by"), JsonPropertyName("order_by")]
            public string OrderBy { get; set; }

            [JsonProperty("page_token"), JsonPropertyName("page_token")]
            public string PageToken { get; set; }

            [JsonProperty("private_extended_property"), JsonPropertyName("private_extended_property")]
            public string PrivateExtendedProperty { get; set; }

            [JsonProperty("q"), JsonPropertyName("q")]
            public string Q { get; set; }

            [JsonProperty("shared_extended_property"), JsonPropertyName("shared_extended_property")]
            public string SharedExtendedProperty { get; set; }

            [JsonProperty("show_deleted"), JsonPropertyName("show_deleted")]
            public bool? ShowDeleted { get; set; }

            [JsonProperty("show_hidden_invitations"), JsonPropertyName("show_hidden_invitations")]
            public bool? ShowHiddenInvitations { get; set; }

            [JsonProperty("single_events"), JsonPropertyName("single_events")]
            public bool? SingleEvents { get; set; }

            [JsonProperty("sync_token"), JsonPropertyName("sync_token")]
            public string SyncToken { get; set; }

            [JsonProperty("time_min"), JsonPropertyName("time_min")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("time_max"), JsonPropertyName("time_max")]
            public DateTime? TimeMax { get; set; }

            [JsonProperty("time_zone"), JsonPropertyName("time_zone")]
            public string TimeZone { get; set; }

            [JsonProperty("updated_min"), JsonPropertyName("updated_min")]
            public DateTime? UpdatedMin { get; set; }
        }
    }
}
