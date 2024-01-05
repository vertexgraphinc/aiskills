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

        [JsonProperty("begin_time")]
        [JsonPropertyName("begin_time")]
        
        public string BeginTime { get; set; }

        [JsonProperty("end_time")]
        [JsonPropertyName("end_time")]
        
        public string EndTime { get; set; }

        [JsonProperty("label")]
        [JsonPropertyName("label")]
        
        public string Label { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        
        [RegularExpression("^(starred|snoozed|read|unread)$", ErrorMessage = "The 'status' property must be 'read' or 'unread'.")]
        public string Status { get; set; }  
    }
}
