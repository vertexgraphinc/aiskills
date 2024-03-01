using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MSTeams.DTOs
{
    public class MSGraphChat
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("@odata.type")]
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdDateTime")]
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonPropertyName("lastUpdatedDateTime")]
        [JsonProperty("lastUpdatedDateTime")]
        public string LastUpdatedDateTime { get; set; }

        [JsonPropertyName("topic")]
        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonPropertyName("chatType")]
        [JsonProperty("chatType")]
        public string ChatType { get; set; }

        [JsonPropertyName("members")]
        [JsonProperty("members")]
        public List<MSGraphMember> Members { get; set; }

        [JsonPropertyName("webUrl")]
        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
    }
}
