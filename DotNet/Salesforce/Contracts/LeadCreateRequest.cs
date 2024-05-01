using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class LeadCreateRequest
    {
        [JsonProperty("FirstName")]
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonProperty("Company")]
        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonProperty("Title")]
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonProperty("Phone")]
        [JsonPropertyName("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Status")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonProperty("Industry")]
        [JsonPropertyName("Industry")]
        public string Industry { get; set; }

        [JsonProperty("LeadSource")]
        [JsonPropertyName("LeadSource")]
        public string LeadSource { get; set; }
    }
}
