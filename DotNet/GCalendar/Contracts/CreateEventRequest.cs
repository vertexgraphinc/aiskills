using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class CreateEventRequest
    {
       
        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("startDateTime"), JsonPropertyName("startDateTime")]
        public string StartDateTime { get; set; }

        [JsonProperty("startTimeZone"), JsonPropertyName("startTimeZone")]
        public string StartTimeZone { get; set; }

        [JsonProperty("endDateTime"), JsonPropertyName("endDateTime")]
        public string EndDateTime { get; set; }

        [JsonProperty("EndTimeZone"), JsonPropertyName("EndTimeZone")]
        public string EndTimeZone { get; set; }

        [JsonProperty("attendees"), JsonPropertyName("attendees")]
        public string Attendees { get; set; }

    }
}
