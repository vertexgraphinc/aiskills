using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Microsoft.Contracts
{
    public class EmailReplyAllRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
