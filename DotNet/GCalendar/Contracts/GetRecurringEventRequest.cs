using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetRecurringEventRequest
    {
        [JsonProperty("originalStart"), JsonPropertyName("originalStart")]
        public string OriginalStart { get; set; }

        [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
        public string TimeMax { get; set; }

        [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
        public string TimeMin { get; set; }
    }
}
