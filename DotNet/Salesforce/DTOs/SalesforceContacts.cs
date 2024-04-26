using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceContacts
    {
        [JsonProperty("records")]
        [JsonPropertyName("records")]
        public List<SalesforceContact> Records { get; set; }
    }
}
