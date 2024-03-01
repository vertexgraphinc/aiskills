using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class TeamUpdateRequest
    {
        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("memberEmails")]
        [JsonPropertyName("memberEmails")]
        public string MemberEmails { get; set; }

        [JsonProperty("updatedDisplayName")]
        [JsonPropertyName("updatedDisplayName")]
        public string UpdatedDisplayName { get; set; }

        [JsonProperty("updatedDescription")]
        [JsonPropertyName("updatedDescription")]
        public string UpdatedDescription { get; set; }
    }
}
