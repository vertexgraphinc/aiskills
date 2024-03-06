using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class ListAllMsgsResponse : ServerResponse
    {
        [JsonProperty("channels"), JsonPropertyName("channels")]
        public List<ListChannelMsgsResponse> Channels { get; set; }
    }
}
