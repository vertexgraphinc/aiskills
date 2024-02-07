using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetRecurringEventRequest
    {
        [JsonProperty("originalStart"), JsonPropertyName("originalStart")]
        public DateTime? OriginalStart { get; set; }

        [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
        public DateTime? TimeMax { get; set; }

        [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
        public DateTime? TimeMin { get; set; }
    }
}
