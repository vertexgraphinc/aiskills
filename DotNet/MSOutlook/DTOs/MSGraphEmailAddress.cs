using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphEmailAddress
    {
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
