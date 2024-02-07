using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class GetEventsRequest
    {
        [JsonProperty("q"), JsonPropertyName("q")]
        public string Q { get; set; }

        [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
        public DateTime TimeMin { get; set; }

        [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
        public DateTime TimeMax { get; set; }
    }
}
