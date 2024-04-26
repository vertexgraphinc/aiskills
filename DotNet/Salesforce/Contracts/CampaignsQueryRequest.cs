using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class CampaignsQueryRequest
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
    }
}
