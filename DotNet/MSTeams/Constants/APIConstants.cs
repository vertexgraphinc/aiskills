﻿namespace MSTeams.Constants
{
    public static class APIConstants
    {
        public const string GraphApiAuthURL = "https://login.microsoftonline.com/";
        public const string GraphApiBaseURL = "https://graph.microsoft.com/v1.0/";
        public const string GraphApiScope = "offline_access User.Read User.ReadBasic.All Chat.ReadWrite ChatMember.ReadWrite Group.ReadWrite.All TeamMember.ReadWrite.All Team.ReadBasic.All";
    }
}
