using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class GMailMessage
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
