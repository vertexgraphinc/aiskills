using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class CampaignsUpdateRequest
    {
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("startDateBeginTime")]
        [JsonPropertyName("startDateBeginTime")]
        public string StartDateBeginTime { get; set; }

        [JsonProperty("startDateEndTime")]
        [JsonPropertyName("startDateEndTime")]
        public string StartDateEndTime { get; set; }

        [JsonProperty("updatedName")]
        [JsonPropertyName("updatedName")]
        public string UpdatedName { get; set; }

        [JsonProperty("updatedType")]
        [JsonPropertyName("updatedType")]
        public string UpdatedType { get; set; }

        [JsonProperty("updatedStatus")]
        [JsonPropertyName("updatedStatus")]
        public string UpdatedStatus { get; set; }

        [JsonProperty("updatedStartDate")]
        [JsonPropertyName("updatedStartDate")]
        public string UpdatedStartDate { get; set; }

        [JsonProperty("updatedEndDate")]
        [JsonPropertyName("updatedEndDate")]
        public string UpdatedEndDate { get; set; }

        [JsonProperty("updatedDescription")]
        [JsonPropertyName("updatedDescription")]
        public string UpdatedDescription { get; set; }
    }
}
