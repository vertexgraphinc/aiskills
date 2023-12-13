using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.Contracts
{
    public class GMailThread: GMailMessage
    {
        //internal purpose. Not listed in yaml
        [JsonProperty("threadId")]
        [JsonPropertyName("threadId")]
        public string ThreadId { get; set; }
    }
}
