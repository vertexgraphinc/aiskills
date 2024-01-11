using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class ForwardEmailRequest
    {
        [JsonProperty("id"),JsonPropertyName("id")]        
        public string Id { get; set; }

        [JsonProperty("new_to"),JsonPropertyName("new_to")]        
        public string NewTo { get; set; }

        [JsonProperty("new_cc"),JsonPropertyName("new_cc")]        
        public string NewCC { get; set; }

        [JsonProperty("new_bcc"),JsonPropertyName("new_bcc")]        
        public string NewBCC { get; set; }
          
    }
}
