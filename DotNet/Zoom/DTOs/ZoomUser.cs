using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Zoom.DTOs
{
    public class ZoomUser
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonProperty("display_name")]
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("company")]
        [JsonPropertyName("company")]
        public string Company { get; set; }
    }
}
