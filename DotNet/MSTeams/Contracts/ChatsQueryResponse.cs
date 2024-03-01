using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatsQueryResponse
    {
        [JsonProperty("chats")]
        [JsonPropertyName("chats")]
        public List<ChatResponse> Chats { get; set; }
    }
}
