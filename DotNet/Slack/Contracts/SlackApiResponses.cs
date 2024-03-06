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

        [JsonProperty("is_im"), JsonPropertyName("is_im")]
        public bool IsDM { get; set; }

        [JsonProperty("user"), JsonPropertyName("user")]
        public string UserId { get; set; }
    }

    public class ChannelsResponse: ApiResult
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

    public class SlackSearchMsgsResponse : ApiResult
    {

        [JsonProperty("messages"), JsonPropertyName("messages")]
        public SearchMessagesResponse MessagesData { get; set; }

    }

    public class UserListResponse : ApiResult
    {

        [JsonProperty("members"), JsonPropertyName("members")]
        public List<UserInfo> Members { get; set; }

    }


    public class SlackUserData
    {
        [JsonProperty("ok"), JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonProperty("sub"), JsonPropertyName("sub")]
        public string Sub { get; set; }

        [JsonProperty("https://slack.com/user_id"), JsonPropertyName("https://slack.com/user_id")]
        public string UserId { get; set; }

        [JsonProperty("https://slack.com/team_id"), JsonPropertyName("https://slack.com/team_id")]
        public string TeamId { get; set; }
    }

    public class ProfileData
    {
        [JsonProperty("status_text"), JsonPropertyName("status_text")]
        public string StatusText { get; set; }

        [JsonProperty("status_emoji"), JsonPropertyName("status_emoji")]
        public string StatusEmoji { get; set; }

        [JsonProperty("status_expiration"), JsonPropertyName("status_expiration")]
        public string StatusExpiration { get; set; }
    }

    public class SlackProfileData : ApiResult
    {
        [JsonProperty("profile"), JsonPropertyName("profile")]
        public ProfileData Profile { get; set; }
    }


    public class UserInfo
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

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

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonProperty("user"), JsonPropertyName("user")]
        public string User { get; set; }

        [JsonProperty("ts"), JsonPropertyName("ts")]
        public string Timestamp { get; set; }
    }

    
}

