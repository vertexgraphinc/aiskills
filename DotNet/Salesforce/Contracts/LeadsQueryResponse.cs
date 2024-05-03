using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class LeadsQueryResponse : ServerResponse
    {
        [JsonProperty("leads")]
        [JsonPropertyName("leads")]
        public List<LeadResponse> Leads { get; set; }

    }
}
