using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Slack.Contracts
{
    public class ListChannelMsgsRequest
    {
        [JsonProperty("channel"), JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonProperty("latest"), JsonPropertyName("latest")]
        public string Latest { get; set; }

        [JsonProperty("oldest"), JsonPropertyName("oldest")]
        public string Oldest { get; set; }
    }
}
