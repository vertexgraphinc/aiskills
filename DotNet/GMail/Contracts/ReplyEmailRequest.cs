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
    public class ReplyEmailRequest
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }

        [JsonProperty("new_cc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_cc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewCC { get; set; }

        [JsonProperty("new_bcc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_bcc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewBCC { get; set; }

        [JsonProperty("new_subject", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewSubject { get; set; }

        [JsonProperty("new_body", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_body")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewBody { get; set; }

    }
}
