using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMembersQueryResponse : ServerResponse
    {
        [JsonProperty("members")]
        [JsonPropertyName("members")]
        public List<MemberResponse> Members { get; set; }
    }
}
