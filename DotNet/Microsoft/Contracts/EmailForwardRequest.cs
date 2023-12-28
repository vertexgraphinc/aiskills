using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Microsoft.Contracts
{
    public class EmailForwardRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("toRecipients")]
        [JsonProperty("toRecipients")]
        public string ToRecipients { get; set; }

        [JsonPropertyName("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
