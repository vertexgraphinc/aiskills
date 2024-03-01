using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class TeamMembersQueryRequest
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

        [JsonProperty("memberDisplayName")]
        [JsonPropertyName("memberDisplayName")]
        public string MemberDisplayName { get; set; }

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public string role { get; set; }
    }
}
