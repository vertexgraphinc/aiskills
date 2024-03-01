﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMemberAddRequest
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

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
