using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class QueryEmailThreadsRequest
    {
        [JsonProperty("query")]
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonProperty("begin_time")]
        [JsonPropertyName("begin_time")]
        public string BeginTime { get; set; }

        [JsonProperty("end_time")]
        [JsonPropertyName("end_time")]
        public string EndTime { get; set; }

    }
}
