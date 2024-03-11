using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.DTOs
{
    public class MSGraphMembers
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("@odata.count")]
        [JsonProperty("@odata.count")]
        public int ODataCount { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        [JsonProperty("@odata.nextLink")]
        public string ODataNextLink { get; set; }

        [JsonPropertyName("groupTopic")]
        [JsonProperty("groupTopic")]
        public string GroupTopic { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public List<MSGraphMember> Value { get; set; }
    }
}
