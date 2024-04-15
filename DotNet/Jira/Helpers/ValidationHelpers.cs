using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Jira.Helpers
{
    public class ValidationHelpers : ControllerBase
    {
        int _defaultLookBackDays = -365;
        public bool Has(object param)
        {
            if (param == null)
                return false;

            if (param is string)
            {
                return !string.IsNullOrEmpty(param.ToString());
            }
            return true;
        }
        protected string Sanitize(string str)
        {
            if (!Has(str))
            {
                return "";
            }
            //remove things that can break json
            return str.Replace("\r", " ").Replace("\n", " ").Replace("\"", "''").Replace("<", " ").Replace(">", " ");
        }

        public static string ParseUnixToDate(double unixTimestamp, int timeZoneOffset)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)unixTimestamp + timeZoneOffset);
            return dateTimeOffset.ToString("MMM dd 'at' h:mmtt");
        }

        public static string ParseDateToUnix(string dateString)
        {
            if (dateString == null)
            {
                return null;
            }
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(dateString);

            return dateTimeOffset.ToUnixTimeSeconds().ToString();
        }
        protected string TrimIfTooLong(string str, int maxLength)
        {
            if (!Has(str))
            {
                return "";
            }
            if (str.Length > maxLength)
            {
                return str.Substring(0, (maxLength - 1)) + "...";
            }
            return str;
        }
        public bool IsAlphaNumeric(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            System.Diagnostics.Debug.WriteLine("[vertex][IsAlphaNumeric]text:" + text);
            bool result = Regex.IsMatch(text, "^[a-zA-Z0-9]+$");
            System.Diagnostics.Debug.WriteLine("[vertex][IsAlphaNumeric]result:" + result);
            return result;
        } 
    }
}
