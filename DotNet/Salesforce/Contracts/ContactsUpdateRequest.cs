using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class ContactsUpdateRequest
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

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("updatedFirstName")]
        [JsonPropertyName("updatedFirstName")]
        public string UpdatedFirstName { get; set; }

        [JsonProperty("updatedLastName")]
        [JsonPropertyName("updatedLastName")]
        public string UpdatedLastName { get; set; }

        [JsonProperty("updatedPhoneNumber")]
        [JsonPropertyName("updatedPhoneNumber")]
        public string UpdatedPhoneNumber { get; set; }

        [JsonProperty("updatedEmail")]
        [JsonPropertyName("updatedEmail")]
        public string UpdatedEmail { get; set; }

        [JsonProperty("updatedDescription")]
        [JsonPropertyName("updatedDescription")]
        public string UpdatedDescription { get; set; }
    }
}
