using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class OAuthAuthedUser
    {
        [JsonPropertyName("id"), JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("scope"), JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("access_token"), JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type"), JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token"), JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in"), JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
