using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Outlook.Contracts
{
    public class EmailReplyRequest
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("comment")]
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("toRecipients")]
        [JsonProperty("toRecipients")]
        public string ToRecipients { get; set; }
    }
}
