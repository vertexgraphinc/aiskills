using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class GetEmailsResponse : ServerResponse
    {
        [JsonProperty("messages"),JsonPropertyName("messages")]
        public List<GMailMessage> Messages { get; set; }
    }
}
