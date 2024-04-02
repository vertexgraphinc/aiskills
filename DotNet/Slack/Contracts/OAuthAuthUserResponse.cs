using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class OAuthAuthUserResponse
    {
        [JsonPropertyName("ok"), JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("authed_user"), JsonProperty("authed_user")]
        public OAuthAuthedUser AuthedUser { get; set; }
    }
}
