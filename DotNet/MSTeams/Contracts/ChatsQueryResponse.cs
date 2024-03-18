using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MSTeams.Contracts
{
    public class ChatsQueryResponse : ServerResponse
    {
        [JsonProperty("chats")]
        [JsonPropertyName("chats")]
        public List<ChatResponse> Chats { get; set; }
    }
}
