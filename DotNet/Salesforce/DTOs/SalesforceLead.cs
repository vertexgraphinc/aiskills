﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceLead
    {
        [JsonPropertyName("Id")]
        [JsonProperty("Id")]
        public string Id { get; set; }

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

        [JsonPropertyName("AnnualRevenue")]
        [JsonProperty("AnnualRevenue")]
        public string AnnualRevenue { get; set; }

        [JsonPropertyName("Rating")]
        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonPropertyName("Description")]
        [JsonProperty("Description")]
        public string Description { get; set; }

    }
}
