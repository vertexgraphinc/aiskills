using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Zoom.Helpers
{
    public static class UtilityHelper
    {
        public static bool IsDateInThePast(string dateString)
        {
            // Parse the string into a DateTime object
            DateTime parsedDateTime;
            if (!DateTime.TryParse(dateString, out parsedDateTime))
            {
                // Parsing failed
                throw new ArgumentException("Invalid date string format");
            }

            // Compare with the current date and time
            return parsedDateTime < DateTime.Now;
        }
        public static string FormatDateTimeUtc(DateTime dateTime, string format = "yyyy-MM-ddTHH:mm:ssZ")
        {
            return dateTime.ToUniversalTime().ToString(format);
        }

        public static string FormatDate(DateTime dateTime, string format = "yyyy-MM-dd")
        {
            return dateTime.ToUniversalTime().ToString(format);
        }

        public static List<string> GetEmailListFromString(string recipients)
        {
            if (string.IsNullOrEmpty(recipients))
            {
                return new List<string>();
            }

            if (JsonHelper.IsJsonArray(recipients))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<string>>(recipients);
                }
                catch (Exception)
                {

                    return new List<string>();
                }
            }
            else
            {
                return recipients.Split(';').ToList();
            }
        }

    }
}
