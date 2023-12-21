using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Outlook.Contracts
{
    public class EmailResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonProperty("received")]
        [JsonPropertyName("received")]
        public string Received { get; set; }
    }
}
