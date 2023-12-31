﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GMail.GMailClient
{
    public class GMailClientMessagePayload
    {
        [JsonPropertyName("partId")]
        [JsonProperty("partId")]
        public string PartId { get; set; }

        [JsonPropertyName("mimeType")]
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonPropertyName("filename")]
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("headers")]
        [JsonProperty("headers")]
        public List<GMailClientHeader> Headers { get; set; }

        [JsonPropertyName("body")]
        [JsonProperty("body")]
        public GMailClientMessageBody Body { get; set; }

        [JsonPropertyName("parts")]
        [JsonProperty("parts")]
        public List<GMailClientMessagePart> Parts { get; set; }

    }
}
