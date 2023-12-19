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

        [JsonProperty("cc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("cc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CC { get; set; }

        [JsonProperty("bcc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("bcc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string BCC { get; set; }   

        [JsonProperty("thread_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("thread_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ThreadId { get; set; }

        [JsonProperty("in_reply_to_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("in_reply_to_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string InReplyToId { get; set; }

        [JsonProperty("references_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("references_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ReferencesId { get; set; }

        [JsonProperty("subject", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
