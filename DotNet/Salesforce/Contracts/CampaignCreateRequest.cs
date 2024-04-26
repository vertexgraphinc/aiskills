using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class CampaignCreateRequest
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("startDate")]
        [JsonPropertyName("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        [JsonPropertyName("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
