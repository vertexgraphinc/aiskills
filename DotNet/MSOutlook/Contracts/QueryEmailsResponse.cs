﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace MSOutlook.Contracts
{
    public class QueryEmailsResponse
    {
        [JsonProperty("emailMessages")]
        [JsonPropertyName("emailMessages")]
        public List<EmailResponse> EmailMessages { get; set; }
    }
}
