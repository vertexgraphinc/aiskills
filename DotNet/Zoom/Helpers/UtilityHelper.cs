using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Zoom.Helpers
{
    public static class UtilityHelper
    {
        public static bool IsMilitaryHourMinutes(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            //checks if the string is a military hour and minutes combo
            //examples: "23:59", "00:00", "12:34", "1:59", "24:00", "19:60", "07:08", "6:30", "23:30"
            string pat = @"^(2[0-3]|[01]?[0-9]):([0-5][0-9])$";
            bool matches = Regex.IsMatch(str, pat);
            return matches;
        }
        public static bool IsDateInThePast(string dateString)
        {
            DateTime parsedDateTime;
            if (!DateTime.TryParse(dateString, out parsedDateTime))
            {
                throw new ArgumentException("Invalid date string format");
            }
            return parsedDateTime < DateTime.Now;
        }
        public static bool IsUtcDateString(string input)
        {
            string format = "yyyy-MM-ddTHH:mm:ssZ";
            if (DateTime.TryParseExact(input, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out DateTime result))
            {
                return result.Kind == DateTimeKind.Utc;
            }
            return false;
        }
        public static string FormatDateTimeUtc(DateTime dateTime, string format = "yyyy-MM-ddTHH:mm:ssZ")
        {
            return dateTime.ToUniversalTime().ToString(format);
        }
        public static DateTime ParseUtcDateStringToLocal(string utcDateString)
        {
            try
            {
                DateTimeOffset utcDateTimeOffset = DateTimeOffset.Parse(utcDateString);
                DateTimeOffset localDateTimeOffset = utcDateTimeOffset.ToLocalTime();
                return localDateTimeOffset.DateTime;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        public static string TryConvertToZoomDate(string dateTimeStr)
        {
            string returnStr = "";
            if (dateTimeStr == null)
            {
                returnStr = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            if (UtilityHelper.IsUtcDateString(dateTimeStr))
            {
                try
                {
                    returnStr = ParseUtcDateStringToLocal(dateTimeStr).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][TryConvertToZoomDate][ex1]" + ex1.ToString());
                }
            }
            else
            {
                try
                {
                    returnStr = DateTime.Parse(dateTimeStr).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][TryConvertToZoomDate][ex2]" + ex2.ToString());
                }
            }
            return returnStr.Replace("Z", "");
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
