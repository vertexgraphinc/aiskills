using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceCampaign
    {
        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonProperty("IsActive")]
        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("Type")]
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonProperty("Status")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonProperty("StartDate")]
        [JsonPropertyName("StartDate")]
        public string StartDate { get; set; }

        [JsonProperty("EndDate")]
        [JsonPropertyName("EndDate")]
        public string EndDate { get; set; }

        [JsonProperty("Description")]
        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
