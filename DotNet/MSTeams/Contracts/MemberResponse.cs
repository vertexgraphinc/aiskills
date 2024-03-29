﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class MemberResponse
    {
        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("roles")]
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("groupTopic")]
        [JsonPropertyName("groupTopic")]
        public string GroupTopic { get; set; }

        [JsonProperty("visibleHistoryStart")]
        [JsonPropertyName("visibleHistoryStart")]
        public string VisibleHistoryStart { get; set; }
    }
}
