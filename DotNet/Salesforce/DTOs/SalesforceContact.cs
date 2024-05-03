using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.DTOs
{
    public class SalesforceContact
    {

        [JsonProperty("Id")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonProperty("FirstName")]
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonProperty("MobilePhone")]
        [JsonPropertyName("MobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonProperty("Description")]
        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}
