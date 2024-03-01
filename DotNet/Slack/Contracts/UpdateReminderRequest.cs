using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Slack.Contracts
{
    public class UpdateReminderRequest
    {
        [JsonProperty("reminderText"), JsonPropertyName("reminderText")]
        public string ReminderText { get; set; }

        [JsonProperty("time"), JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
