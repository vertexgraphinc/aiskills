using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphMessageBody
    {
        [JsonPropertyName("contentType")]
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("content")]
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
