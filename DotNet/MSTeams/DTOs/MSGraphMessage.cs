using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace MSTeams.DTOs
{
    public class Body
    {
        [JsonPropertyName("contentType")]
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("content")]
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class From
    {
        [JsonPropertyName("user")]
        [JsonProperty("user")]
        public MSGraphMember User { get; set; }
    }

    public class MSGraphMessage
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("chatId")]
        [JsonProperty("chatId")]
        public string ChatId { get; set; }

        [JsonPropertyName("createdDateTime")]
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonPropertyName("lastModifiedDateTime")]
        [JsonProperty("lastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }

        [JsonPropertyName("from")]
        [JsonProperty("from")]
        public From From { get; set; }

        [JsonPropertyName("body")]
        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonPropertyName("messageType")]
        [JsonProperty("messageType")]
        public string MessageType { get; set; }
    }
}
