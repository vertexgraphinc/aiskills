using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class ListChannelMsgsResponse : ServerResponse
    {

        [JsonProperty("channel"), JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonProperty("messages"), JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }
}
