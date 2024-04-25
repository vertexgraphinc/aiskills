using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class UpdateEventRequest
    {
        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("currentSummary"), JsonPropertyName("currentSummary")]
        public string CurrentSummary { get; set; }

        [JsonProperty("currentStartDateTime"), JsonPropertyName("currentStartDateTime")]
        public string CurrentStartDateTime { get; set; }

        [JsonProperty("currentEndDateTime"), JsonPropertyName("currentEndDateTime")]
        public string CurrentEndDateTime { get; set; }

        [JsonProperty("updatedSummary"), JsonPropertyName("updatedSummary")]
        public string UpdatedSummary { get; set; }

        [JsonProperty("updatedDescription"), JsonPropertyName("updatedDescription")]
        public string UpdatedDescription { get; set; }

        [JsonProperty("updatedStartDateTime"), JsonPropertyName("updatedStartDateTime")]
        public string UpdatedStartDateTime { get; set; }

        [JsonProperty("updatedEndDateTime"), JsonPropertyName("updatedEndDateTime")]
        public string UpdatedEndDateTime { get; set; }

        [JsonProperty("attendeesToRemove"), JsonPropertyName("attendeesToRemove")]
        public string AttendeesToRemove { get; set; }

        [JsonProperty("attendeesToAdd"), JsonPropertyName("attendeesToAdd")]
        public string AttendeesToAdd { get; set; }
    }
}
