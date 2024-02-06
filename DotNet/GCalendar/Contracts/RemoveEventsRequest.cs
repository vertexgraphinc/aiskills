using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class RemoveEventsRequest
    {
        [JsonProperty("calendarId"), JsonPropertyName("calendarId")]
        public string CalendarId { get; set; }

        [JsonProperty("eventId"), JsonPropertyName("eventId")]
        public string EventId { get; set; }

        [JsonProperty("optionalParams"), JsonPropertyName("optionalParams")]
        public OptionalParameters OptionalParams { get; set; }

        public class OptionalParameters
        {
            [JsonProperty("sendUpdates"), JsonPropertyName("sendUpdates")]
            public string SendUpdates { get; set; }

            [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
            public DateTime? TimeMin { get; set; }

            [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
            public DateTime? TimeMax { get; set; }
        }
    }

}
