using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class OAuthError
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]

        public string Code { get; set; }

        [JsonPropertyName("error")]
        [JsonProperty("error")]

        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        [JsonProperty("error_description")]

        public string ErrorDescription { get; set; }
    }
}
