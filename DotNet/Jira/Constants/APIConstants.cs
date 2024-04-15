namespace Jira.Constants
{
    public static class APIConstants
    {
        public const string ApiAuthURL = "https://auth.atlassian.com/oauth/token";
        public const string ApiBaseURL = "https://auth.atlassian.com/authorize";
        public const string ApiScope = "read:jira-work write:jira-work read:jira-user";
    }
}
