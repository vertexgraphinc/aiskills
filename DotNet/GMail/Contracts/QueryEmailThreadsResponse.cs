using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class QueryEmailThreadsResponse : ServerResponse
    {
        [JsonProperty("emailThreads")]
        [JsonPropertyName("emailThreads")]
        public List<GMailThread> EmailThreads { get; set; }
    }
}
