using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace MSOutlook.Helpers
{
    public static class UtilityHelper
    {
        public static string FormatDateTimeUtc(DateTime dateTime, string format = "yyyy-MM-ddTHH:mm:ssZ")
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
        public static bool Has(object param)
        {
            if (param == null)
                return false;

            if (param is string)
            {
                return !string.IsNullOrEmpty(param.ToString());
            }
            return true;
        }
        public static string Sanitize(string str)
        {
            if (!Has(str))
            {
                return "";
            }
            //remove things that can break json
            return str.Replace("\r", " ").Replace("\n", " ").Replace("\"", "''").Replace("<", " ").Replace(">", " ");
        }
        public static string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }

            html = html.Replace("\u00A0", " ");//unicode non-breaking spaces    
            html = html.Replace("&nbsp;", " ");//html non-breaking spaces
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            string txt = doc.DocumentNode.InnerText;
            //strip "![test](https://example.com/test.jpg)" to just "https://example.com/test.jpg"
            string pattern = @"!\[[^\]*]\]\((https?://[^)]+)\)";
            txt = Regex.Replace(txt, pattern, "\n$1");

            //strip "!][(https://example.com/test.jpg)" to just "https://example.com/test.jpg"
            pattern = @"!\]\[\((https?://[^)]+)\)";
            txt = Regex.Replace(txt, pattern, "\n$1");

            //replace all multiple line breaks with double line break
            txt = Regex.Replace(txt, @"(\r\n|\n\r|\n|\r){2,}", "\n\n");

            txt = txt.Replace("\"", "&quot;");
            txt = txt.Replace("'", "&apos;");
            txt = txt.Replace("{", "&#123;");
            txt = txt.Replace("}", "&#125;");
            txt = txt.Replace("[", "&#91;");
            txt = txt.Replace("]", "&#93;");
            txt = txt.Replace("(", "&#40;");
            txt = txt.Replace(")", "&#41;");

            return txt;
        }
    }
}
