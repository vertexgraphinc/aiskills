﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMemberAddRequest
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
