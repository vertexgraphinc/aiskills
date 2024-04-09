using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zoom.Contracts
{
    public class MeetingChatsQueryResponse : ServerResponse
    {
        [JsonProperty("chats")]
        [JsonPropertyName("chats")]
        public List<MeetingChatResponse> Chats { get; set; }
    }
}
