using System;

namespace Jira.Helpers
{
    public class TokenHelper
    {
        public static string GetSessionToken(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
                return null;

            string[] parts = authorizationHeader.Split(' ');
            if (parts.Length == 1)
                return authorizationHeader;

            if (string.Equals(parts[0], "Bearer", StringComparison.OrdinalIgnoreCase))
                return parts[1];

            return authorizationHeader;
        }
    }
}
