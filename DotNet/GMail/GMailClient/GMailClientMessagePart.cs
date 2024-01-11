using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GMail.GMailClient
{
    public class GMailClientMessagePart
    {
        [JsonProperty("partId"),JsonPropertyName("partId")]
        public string PartId { get; set; }

        [JsonProperty("mimeType"),JsonPropertyName("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("filename"),JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonProperty("headers"),JsonPropertyName("headers")]
        public List<GMailClientHeader> Headers { get; set; }

        [JsonProperty("body"),JsonPropertyName("body")]
        public GMailClientMessageBody Body { get; set; }

        [JsonProperty("parts"),JsonPropertyName("parts")]
        public List<GMailClientMessagePart> Parts { get; set; }
    }
}