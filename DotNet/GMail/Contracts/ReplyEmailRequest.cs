using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class ReplyEmailRequest
    {
        [JsonProperty("id"),JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("new_cc"),JsonPropertyName("new_cc")]
        public string NewCC { get; set; }

        [JsonProperty("new_bcc"),JsonPropertyName("new_bcc")]
        public string NewBCC { get; set; }

        [JsonProperty("new_subject"),JsonPropertyName("new_subject")]
        public string NewSubject { get; set; }

        [JsonProperty("new_body"),JsonPropertyName("new_body")]
        public string NewBody { get; set; }

    }
}
