using GMail.GMailClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class GetEmailsResponse : ServerResponse
    {
        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public List<GMailMessage> Messages { get; set; }
    }
}
