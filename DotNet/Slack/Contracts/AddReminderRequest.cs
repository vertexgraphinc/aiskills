using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Slack.Contracts
{
    public class AddReminderRequest
    {
        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonProperty("time"), JsonPropertyName("time")]
        public string Time { get; set; }

    }
}
