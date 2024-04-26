using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Salesforce.Contracts
{
    public class ContactsQueryResponse : ServerResponse
    {
        [JsonProperty("contacts")]
        [JsonPropertyName("contacts")]
        public List<ContactResponse> Contacts { get; set; }
    }
}
