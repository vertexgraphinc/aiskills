using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class ContactCreateRequest
    {
        [JsonProperty("firstName")]
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phoneNumber")]
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
