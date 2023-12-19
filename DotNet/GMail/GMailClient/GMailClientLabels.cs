using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.GMailClient
{
    public class GMailClientLabels
    {
        [JsonProperty("labels")]
        [JsonPropertyName("labels")]
        public List<GMailClientLabel> Labels { get; set; }
    }
}
