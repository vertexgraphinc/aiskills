using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Zoom.Helpers
{
    public static class UtilityHelper
    {
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
