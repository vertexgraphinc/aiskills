using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SearchMessagesResponse : ServerResponse
    {
        [JsonProperty("messages"), JsonPropertyName("messages")]
        public MessagesData Messages { get; set; }

        [JsonProperty("ok"), JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonProperty("error"), JsonPropertyName("error")]
        public string Error { get; set; }
        public class MessagesData
        {
            [JsonProperty("matches"), JsonPropertyName("matches")]
            public List<MessageMatch> Matches { get; set; }
        }

        public class MessageMatch
        {
            [JsonProperty("channel"), JsonPropertyName("channel")]
            public ChannelInfo Channel { get; set; }

            [JsonProperty("type"), JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonProperty("username"), JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonProperty("ts"), JsonPropertyName("ts")]
            public string Timestamp { get; set; }

            [JsonProperty("text"), JsonPropertyName("text")]
            public string Text { get; set; }

            [JsonProperty("permalink"), JsonPropertyName("permalink")]
            public string Permalink { get; set; }
        }

        public class ChannelInfo
        {
            [JsonProperty("name"), JsonPropertyName("name")]
            public string Name { get; set; }
        }
    }
}
