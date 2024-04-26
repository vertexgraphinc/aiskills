using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class CampaignCreateResponse : ServerResponse
    {
        [JsonProperty("campaign")]
        [JsonPropertyName("campaign")]
        public CampaignResponse Campaign { get; set; }
    }
}
