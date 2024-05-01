using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceLeads
    {
        [JsonProperty("records")]
        [JsonPropertyName("records")]
        public List<SalesforceLead> Records { get; set; }
    }
}
