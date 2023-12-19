using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientHeader
    {
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}