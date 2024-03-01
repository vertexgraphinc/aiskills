using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatMessagesQueryResponse
    {
        [JsonProperty("messages")]
        [JsonPropertyName("messages")]
        public List<MessageResponse> Messages { get; set; }
    }
}
