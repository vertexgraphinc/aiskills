using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class OAuthRefreshTokenResponse
    {
        private string _expiresIn = null;

        [JsonPropertyName("ok"),JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("access_token"), JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in"), JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token"), JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("token_type"), JsonProperty("token_type")]
        public string TokenType { get; set; }  
    }
}
