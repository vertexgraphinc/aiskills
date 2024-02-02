using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class RemoveEventsRequest
    {
        [JsonProperty("calendar_id"), JsonPropertyName("calendar_id")]
        public string CalendarId { get; set; }

        [JsonProperty("event_id"), JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonProperty("optional_params"), JsonPropertyName("optional_params")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("send_updates"), JsonPropertyName("send_updates")]
            public string SendUpdates { get; set; }

            [JsonProperty("time_min"), JsonPropertyName("time_min")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("time_max"), JsonPropertyName("time_max")]
            public DateTime? TimeMax { get; set; }
        }
    }
}
