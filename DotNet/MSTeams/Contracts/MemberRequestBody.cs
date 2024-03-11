using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class MemberRequestBody
    {
        [JsonPropertyName("@odata.type")]
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        [JsonProperty("roles")]
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonPropertyName("user@odata.bind")]
        [JsonProperty("user@odata.bind")]
        public string UserODataBind { get; set; }

        [JsonProperty("visibleHistoryStartDateTime")]
        [JsonPropertyName("visibleHistoryStartDateTime")]
        public string VisibleHistoryStartDateTime { get; set; }
    }
}
