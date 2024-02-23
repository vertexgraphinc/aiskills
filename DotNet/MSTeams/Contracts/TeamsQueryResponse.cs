using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class TeamsQueryResponse
    {
        [JsonProperty("teams")]
        [JsonPropertyName("teams")]
        public List<TeamResponse> Teams { get; set; }
    }
}
