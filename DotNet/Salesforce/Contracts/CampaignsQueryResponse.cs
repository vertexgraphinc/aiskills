using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class CampaignsQueryResponse : ServerResponse
    {
        [JsonProperty("campaigns")]
        [JsonPropertyName("campaigns")]
        public List<CampaignResponse> Campaigns { get; set; }

    }
}
