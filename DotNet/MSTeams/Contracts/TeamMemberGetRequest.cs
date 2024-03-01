using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class TeamMemberGetRequest
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("memberId")]
        [JsonPropertyName("memberId")]
        public string MemberId { get; set; }
    }
}
