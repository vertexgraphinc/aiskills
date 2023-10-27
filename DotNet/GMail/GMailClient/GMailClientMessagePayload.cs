using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientMessagePayload
    {
        [JsonPropertyName("mimeType")]
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("headers")]
        [JsonProperty("headers")]
        public List<GMailClientHeader> Headers { get; set; }

    }
}
