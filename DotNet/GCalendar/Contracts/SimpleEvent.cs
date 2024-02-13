using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class SimpleEvent
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("location"), JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonProperty("startDateTime"), JsonPropertyName("startDateTime")]
        public string StartDateTime { get; set; }

        [JsonProperty("startTimeZone"), JsonPropertyName("startTimeZone")]
        public string StartTimeZone { get; set; }

        [JsonProperty("endDateTime"), JsonPropertyName("endDateTime")]
        public string EndDateTime { get; set; }

        [JsonProperty("endTimeZone"), JsonPropertyName("endTimeZone")]
        public string EndTimeZone { get; set; }

        [JsonProperty("recurrence"), JsonPropertyName("recurrence")]
        public string Recurrence { get; set; }

        [JsonProperty("recurringEventId"), JsonPropertyName("recurringEventId")]
        public string RecurringEventId { get; set; }

        [JsonProperty("originalStartDateTime"), JsonPropertyName("originalStartDateTime")]
        public string OriginalStartDateTime { get; set; }

        [JsonProperty("originalStartTimeZone"), JsonPropertyName("originalStartTimeZone")]
        public string OriginalStartTimeZone { get; set; }

        [JsonProperty("attendeesEmails"), JsonPropertyName("attendeesEmails")]
        public string AttendeesEmails { get; set; }
    }
}
