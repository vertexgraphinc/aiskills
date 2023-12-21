﻿using GMail.GMailClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class QueryEmailAndAddLabelRequest : SearchFilters
    {
        [JsonProperty("add_label")]
        [JsonPropertyName("add_label")]
        public string AddLabel { get; set; }

        public SearchFilters GetSearchFilters()
        {
            var sf = new SearchFilters();
            sf.From = this.From;
            sf.To = this.To;
            sf.Subject = this.Subject;
            sf.Body = this.Body;
            sf.Status = this.Status;
            sf.Label = this.Label;
            sf.BeginTime = this.BeginTime;
            sf.EndTime = this.EndTime;
            sf.Status = this.Status;
            return sf;
        }
    }
}
