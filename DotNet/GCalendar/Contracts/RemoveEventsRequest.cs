using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class RemoveEventsRequest
    {
        [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
        public DateTime? TimeMin { get; set; }

        [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
        public DateTime? TimeMax { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }

}
