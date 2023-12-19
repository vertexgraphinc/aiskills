using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientMessageBody
    {
        [JsonPropertyName("size")]
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonPropertyName("data")]
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}