using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class GetEmailRequestById
    {
        [JsonProperty("id"),JsonPropertyName("id")]        
        public string Id { get; set; }
    }
}
