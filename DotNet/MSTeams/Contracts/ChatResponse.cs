using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("tenantId")]
        [JsonPropertyName("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("topic")]
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        [JsonProperty("chatType")]
        [JsonPropertyName("chatType")]
        public string ChatType { get; set; }

        [JsonProperty("memberEmails")]
        [JsonPropertyName("memberEmails")]
        public string MemberEmails { get; set; }

        [JsonProperty("lastUpdated")]
        [JsonPropertyName("lastUpdated")]
        public string LastUpdated { get; set; }

        [JsonProperty("webUrl")]
        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; }
    }
}
