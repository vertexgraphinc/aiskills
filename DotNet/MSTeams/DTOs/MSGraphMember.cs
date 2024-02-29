using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.DTOs
{
    public class MSGraphMember
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

        [JsonPropertyName("userId")]
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("roles")]
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("visibleHistoryStartDateTime")]
        [JsonPropertyName("visibleHistoryStartDateTime")]
        public string VisibleHistoryStartDateTime { get; set; }
    }
}
