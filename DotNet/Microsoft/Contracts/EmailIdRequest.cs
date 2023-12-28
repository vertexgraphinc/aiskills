using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Microsoft.Contracts
{
    public class EmailIdRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
