using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Contracts
{
    public class EmailRequest
    {
        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("toRecipients")]
        [JsonProperty("toRecipients")]
        public string ToRecipients { get; set; }

        [JsonPropertyName("ccRecipients")]
        [JsonProperty("ccRecipients")]
        public string CCRecipients { get; set; }

        [JsonPropertyName("bccRecipients")]
        [JsonProperty("bccRecipients")]
        public string BCCRecipients { get; set; }
    }
}
