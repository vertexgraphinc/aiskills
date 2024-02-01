using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GCalendar.Contracts
{
    public class OAuthTokenPara
    {
        [JsonProperty("grant_type"),JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("client_id"),JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret"),JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("code"),JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonProperty("redirect_uri"),JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
