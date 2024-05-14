namespace Jira.Constants
{
    public static class APIConstants
    {
        public const string ApiTokenURL = "https://auth.atlassian.com/oauth/token";
        public const string ApiAuthURL = "https://auth.atlassian.com/authorize";
        public const string ApiInfoUrl = "https://api.atlassian.com/oauth/token/accessible-resources";

        public const string ApiScope = "read:jira-work write:jira-work read:jira-user";

        public const string ApiBaseURL = "https://api.atlassian.com/ex/jira/";
        public const string ApiRestURL = "/rest/api/3/";
    }
}
