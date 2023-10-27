using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientHeader
    {
        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
