using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceCampaigns
    {
        [JsonProperty("records")]
        [JsonPropertyName("records")]
        public List<SalesforceCampaign> Records { get; set; }
    }
}
