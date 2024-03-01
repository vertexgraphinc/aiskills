using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Slack.Contracts
{
    public class ListAllMsgsRequest
    {
        [JsonProperty("timeMax"), JsonPropertyName("timeMax")]
        public DateTime? TimeMax { get; set; }

        [JsonProperty("timeMin"), JsonPropertyName("timeMin")]
        public DateTime? TimeMin { get; set; }
    }
}
