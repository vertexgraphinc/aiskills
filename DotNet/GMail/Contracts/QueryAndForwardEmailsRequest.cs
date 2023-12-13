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
    public class QueryAndForwardEmailsRequest : SearchFilters
    {
        [JsonProperty("new_to", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_to")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewTo { get; set; }

        [JsonProperty("new_cc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_cc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewCC { get; set; }

        [JsonProperty("new_bcc", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("new_bcc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string NewBCC { get; set; }
    }
}
