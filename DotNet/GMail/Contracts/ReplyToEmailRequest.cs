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
    public class ReplyToEmailRequest
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }

        [JsonProperty("new_subject", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewSubject { get; set; }

        [JsonProperty("new_body")]
        [JsonPropertyName("new_body")]
        public string NewBody { get; set; }
    }
}
