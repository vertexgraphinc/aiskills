using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.Contracts
{
    public class QueryEmailsRequest
    {
        [JsonProperty("begin_time")]
        [JsonPropertyName("begin_time")]
        public string BeginTime { get; set; }

        [JsonProperty("end_time")]
        [JsonPropertyName("end_time")]
        public string EndTime { get; set; }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("importance")]
        [JsonPropertyName("importance")]
        public string Importance { get; set; }

        [JsonProperty("has_attachments")]
        [JsonPropertyName("has_attachments")]
        public bool? HasAttachments { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
