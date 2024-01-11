using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class GetEmailRequest
    {
        [JsonProperty("id"),JsonPropertyName("id")]        
        public string Id { get; set; }
    }
}
