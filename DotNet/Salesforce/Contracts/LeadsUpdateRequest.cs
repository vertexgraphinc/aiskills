using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class LeadsUpdateRequest
    {

        [JsonPropertyName("FirstName")]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("Email")]
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Company")]
        [JsonProperty("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Phone")]
        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonPropertyName("Status")]
        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonPropertyName("Industry")]
        [JsonProperty("Industry")]
        public string Industry { get; set; }

        [JsonPropertyName("LeadSource")]
        [JsonProperty("LeadSource")]
        public string LeadSource { get; set; }

        [JsonPropertyName("MinAnnualRevenue")]
        [JsonProperty("MinAnnualRevenue")]
        public string MinAnnualRevenue { get; set; }

        [JsonPropertyName("MaxAnnualRevenue")]
        [JsonProperty("MaxAnnualRevenue")]
        public string MaxAnnualRevenue { get; set; }

        [JsonPropertyName("Rating")]
        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonPropertyName("Description")]
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonPropertyName("UpdatedFirstName")]
        [JsonProperty("UpdatedFirstName")]
        public string UpdatedFirstName { get; set; }

        [JsonPropertyName("UpdatedLastName")]
        [JsonProperty("UpdatedLastName")]
        public string UpdatedLastName { get; set; }

        [JsonPropertyName("UpdatedEmail")]
        [JsonProperty("UpdatedEmail")]
        public string UpdatedEmail { get; set; }

        [JsonPropertyName("UpdatedCompany")]
        [JsonProperty("UpdatedCompany")]
        public string UpdatedCompany { get; set; }

        [JsonPropertyName("UpdatedPhone")]
        [JsonProperty("UpdatedPhone")]
        public string UpdatedPhone { get; set; }

        [JsonPropertyName("UpdatedStatus")]
        [JsonProperty("UpdatedStatus")]
        public string UpdatedStatus { get; set; }

        [JsonPropertyName("UpdatedIndustry")]
        [JsonProperty("UpdatedIndustry")]
        public string UpdatedIndustry { get; set; }

        [JsonPropertyName("UpdatedLeadSource")]
        [JsonProperty("UpdatedLeadSource")]
        public string UpdatedLeadSource { get; set; }

        [JsonPropertyName("UpdatedAnnualRevenue")]
        [JsonProperty("UpdatedAnnualRevenue")]
        public string UpdatedAnnualRevenue { get; set; }

        [JsonPropertyName("UpdatedRating")]
        [JsonProperty("UpdatedRating")]
        public string UpdatedRating { get; set; }

        [JsonPropertyName("UpdatedDescription")]
        [JsonProperty("UpdatedDescription")]
        public string UpdatedDescription { get; set; }
    }
}
