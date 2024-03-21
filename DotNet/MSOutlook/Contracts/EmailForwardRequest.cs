﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace MSOutlook.Contracts
{
    public class EmailForwardRequest
    {
        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("toRecipients")]
        [JsonProperty("toRecipients")]
        public string ToRecipients { get; set; }

        [JsonPropertyName("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
