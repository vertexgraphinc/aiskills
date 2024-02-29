using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.DTOs
{
    public class MSGraphUserEntity
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("businessPhones")]
        [JsonProperty("businessPhones")]
        public List<string> BusinessPhones { get; set; }

        [JsonPropertyName("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("givenName")]
        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("jobTitle")]
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("mail")]
        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonPropertyName("mobilePhone")]
        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }

        [JsonPropertyName("officeLocation")]
        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonPropertyName("preferredLanguage")]
        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }

        [JsonPropertyName("surname")]
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("userPrincipalName")]
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }
    }
}
