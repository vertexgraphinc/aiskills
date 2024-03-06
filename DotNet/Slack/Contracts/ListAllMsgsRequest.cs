using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace Slack.Contracts
{
    public class ListAllMsgsRequest
    {
        [JsonProperty("types"), JsonPropertyName("types")]
        public string Types { get; set; }

        [JsonProperty("limit"), JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonProperty("oldest"), JsonPropertyName("oldest")]
        public string Oldest { get; set; }

        [JsonProperty("latest"), JsonPropertyName("latest")]
        public string Latest { get; set; }
    }
}
