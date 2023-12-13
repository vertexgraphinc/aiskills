using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class QueryEmailsResponse : ServerResponse
    {
        [JsonProperty("emailMessages")]
        [JsonPropertyName("emailMessages")]
        public List<GMailMessage> EmailMessages { get; set; }
    }
}
