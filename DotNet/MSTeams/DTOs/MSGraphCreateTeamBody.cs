using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.DTOs
{
    public class MSGraphCreateTeamBody
    {
        [JsonPropertyName("template@odata.bind")]
        [JsonProperty("template@odata.bind")]
        public string TemplateODataBind { get; set; }

        [JsonPropertyName("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonPropertyName("members")]
        [JsonProperty("members")]
        public List<MSGraphMember> Members { get; set; }
    }
}
