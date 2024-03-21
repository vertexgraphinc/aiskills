using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.Contracts
{
    public class EmailReplyAllRequest
    {
        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
