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
    public class SearchFilters
    {
        [JsonProperty("from", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("from")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string From { get; set; }

        [JsonProperty("to", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("to")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string To { get; set; }

        [JsonProperty("subject", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Subject { get; set; }

        [JsonProperty("body", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("body")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Body { get; set; }

        [JsonProperty("begin_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("begin_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string BeginTime { get; set; }

        [JsonProperty("end_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("end_time")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string EndTime { get; set; }

        [JsonProperty("label", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("label")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Label { get; set; }

        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [RegularExpression("^(starred|snoozed|read|unread)$", ErrorMessage = "The 'status' property must be 'read' or 'unread'.")]
        public string Status { get; set; }  
    }
}
