using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMembersQueryRequest
    {
        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("chatType")]
        [JsonPropertyName("chatType")]
        public string ChatType { get; set; }

        [JsonProperty("memberEmails")]
        [JsonPropertyName("memberEmails")]
        public string MemberEmails { get; set; }

        [JsonProperty("lastUpdatedBeginTime")]
        [JsonPropertyName("lastUpdatedBeginTime")]
        public string LastUpdatedBeginTime { get; set; }

        [JsonProperty("lastUpdatedEndTime")]
        [JsonPropertyName("lastUpdatedEndTime")]
        public string LastUpdatedEndTime { get; set; }

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("role")]
        [JsonPropertyName("role")]
        public string role { get; set; }
    }
}
