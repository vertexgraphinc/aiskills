using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.Contracts
{
    public class EmailIdRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
