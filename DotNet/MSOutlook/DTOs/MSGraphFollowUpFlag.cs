using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphFollowUpFlag
    {
        [JsonPropertyName("completedDateTime")]
        [JsonProperty("completedDateTime")]
        public MSGraphDateTimeTimeZone CompletedDateTime { get; set; }

        [JsonPropertyName("dueDateTime")]
        [JsonProperty("dueDateTime")]
        public MSGraphDateTimeTimeZone DueDateTime { get; set; }

        [JsonPropertyName("flagStatus")]
        [JsonProperty("flagStatus")]
        public string FlagStatus { get; set; }

        [JsonPropertyName("startDateTime")]
        [JsonProperty("startDateTime")]
        public MSGraphDateTimeTimeZone startDateTime { get; set; }
    }
}