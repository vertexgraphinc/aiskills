using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class TeamResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("tenantId")]
        [JsonPropertyName("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("memberEmails")]
        [JsonPropertyName("memberEmails")]
        public string MemberEmails { get; set; }

        [JsonProperty("webUrl")]
        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; }
    }
}
