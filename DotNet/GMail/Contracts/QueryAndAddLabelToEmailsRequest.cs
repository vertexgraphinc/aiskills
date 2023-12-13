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
    public class QueryAndAddLabelToEmailsRequest : SearchFilters
    {
        [JsonProperty("add_label")]
        [JsonPropertyName("add_label")]
        public string AddLabel { get; set; }
    }
}
