namespace MSOutlook.Constants
{
    public static class APIConstants
    {
        public const string GraphApiAuthURL = "https://login.microsoftonline.com/";
        public const string GraphApiBaseURL = "https://graph.microsoft.com/v1.0/me/";
        public const string GraphApiScope = "offline_access User.Read Mail.ReadWrite Mail.Send";
    }
}
