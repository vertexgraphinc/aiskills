using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SlackApiResponses
    {
        

    }

    public class ApiResult
    {

        [JsonProperty("ok"), JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonProperty("error"), JsonPropertyName("error")]
        public string Error { get; set; }
    }
    public class Channel
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ChannelsResponse
    {

        [JsonProperty("channels"), JsonPropertyName("channels")]
        public List<Channel> Channels { get; set; }

    }

    public class HistoryResponse : ApiResult
    {

        [JsonProperty("messages"), JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

    }

    public class UserInfoResponse : ApiResult
    {

        [JsonProperty("user"), JsonPropertyName("user")]
        public UserInfo User { get; set; }

    }

    public class UserInfo
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("real_name"), JsonPropertyName("real_name")]
        public string RealName { get; set; }

        [JsonProperty("tz"), JsonPropertyName("tz")]
        public string TimeZone { get; set; }

        [JsonProperty("tz_label"), JsonPropertyName("tz_label")]
        public string TimeZoneLabel { get; set; }

        [JsonProperty("tz_offset"), JsonPropertyName("tz_offset")]
        public int TimeZoneOffset { get; set; }
    }

    public class Message
    {
        [JsonProperty("channel"), JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonProperty("user"), JsonPropertyName("user")]
        public string User { get; set; }

        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("ts"), JsonPropertyName("ts")]
        public string Timestamp { get; set; }
    }

    
}

