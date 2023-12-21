using System;
using System.Collections.Generic;
using System.Linq;

namespace Outlook.Helpers
{
    public static class UtilityHelper
    {
        public static string FormatDateTimeUtc(DateTime dateTime, string format = "yyyy-MM-ddTHH:mm:ssZ")
        {
            return dateTime.ToUniversalTime().ToString(format);
        }

        public static List<string> GetEmailListFromString(string recipients) {
            if (string.IsNullOrEmpty(recipients))
            {
                return new List<string>();
            }

            return recipients.Split(';').ToList();
        }

    }
}
