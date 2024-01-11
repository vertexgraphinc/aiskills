using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class RemoveLabelRequest
    {
        [JsonProperty("id"),JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("label"),JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
