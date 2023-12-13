﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.Contracts
{
    public class QueryAndRemoveLabelFromEmailsRequest : SearchFilters
    {
        [JsonProperty("remove_label")]
        [JsonPropertyName("remove_label")]
        public string RemoveLabel { get; set; }
    }
}
