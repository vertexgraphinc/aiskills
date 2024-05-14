using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Zoom.Contracts
{
    public class ServerError
    {
        [JsonPropertyName("code")]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
