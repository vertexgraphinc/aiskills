using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMembersQueryRequest
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public string role { get; set; }
    }
}
