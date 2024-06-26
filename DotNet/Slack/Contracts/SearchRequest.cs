﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SearchRequest
    {
        [JsonProperty("query"), JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonProperty("memberEmail"), JsonPropertyName("memberEmail")]
        public string MemberEmail { get; set; }
    }
}
