namespace Slack.Constants
{
    public static class APIConstants
    {
        public const string ApiAuthURL = "https://slack.com/api/oauth.v2.access";
        public const string ApiBaseURL = "https://slack.com/oauth/v2/authorize";
        public const string ApiScope = "users.profile:read,users:read.email,channels:history,groups:history,im:history,mpim:history,channels:read,chat:write,dnd:write,groups:read,im:read,mpim:read,reminders:read,reminders:write,search:read,users.profile:write,users:read";
    }
}
