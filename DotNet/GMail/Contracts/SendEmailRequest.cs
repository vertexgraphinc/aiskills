using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.Contracts
{
    public class SendEmailRequest
    {
        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("cc")]
        [JsonPropertyName("cc")]
        public string CC { get; set; }

        [JsonProperty("bcc")]
        [JsonPropertyName("bcc")]
        public string BCC { get; set; }   

        [JsonProperty("thread_id")]
        [JsonPropertyName("thread_id")]
        public string ThreadId { get; set; }

        [JsonProperty("in_reply_to_id")]
        [JsonPropertyName("in_reply_to_id")]
        public string InReplyToId { get; set; }

        [JsonProperty("references_id")]
        [JsonPropertyName("references_id")]
        public string ReferencesId { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
