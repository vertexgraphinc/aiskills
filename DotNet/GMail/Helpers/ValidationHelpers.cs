using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace GMail.Helpers
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
        public string GetOptionalQStringParam(string paramName, string paramValue)
        {
            //https://support.google.com/mail/answer/7190
            //returns the formatted search operator in the form: " paramName:\"paramValue\""
            if (Has(paramName) && Has(paramValue))
            {
                //                                        unlike System.Net.WebUtility.UrlEncode, Uri.EscapeDataString encodes spaces as %20 as opposed to +
                return string.Concat(" ", paramName, ":\"", Uri.EscapeDataString(paramValue), "\"");
            }
            return "";
        }
        protected bool HasAtLeastOneProp(string[] props)
        {
            if (props == null)
            {
                return false;
            }
            if (props.Length == 0)
            {
                return false;
            }
            foreach (string prop in props)
            {
                if (Has(prop))
                {
                    return true;
                }
            }
            return false;
        }
        public string GetBodyQuery(string bodyParam)
        {
            //returns the q querystring in the form: q="bodyParamHere"
            string q = "q=";
            if (Has(bodyParam))
            {
                q += string.Concat("\"", bodyParam.Replace("\"", "&quot;"), "\" ");
            }
            else
            {
                q += "";
            }
            return q;
        }
        public string AssembleSearchParams(string from, string label, string subject, string body, string status, string dateQuery)
        {
            string searchParams = string.Concat(from, label, subject, status);
            searchParams = string.Concat(body, searchParams.Trim());
            if (Has(searchParams))
            {
                if (Has(dateQuery))
                {
                    searchParams = string.Concat(searchParams.Trim(), " ", dateQuery.Trim());
                }
            }
            else
            {
                if (Has(dateQuery))
                {
                    searchParams = dateQuery;
                }
            }
            return searchParams;
        }
        protected string GetSentQuery(string begin, string end)
        {
            string dateQuery = "";
            string strDt = ParsedShortDate(begin, DateTime.Now.AddDays(_defaultLookBackDays));
            string endDt = ParsedShortDate(end, DateTime.Now.AddDays(1));
            if (!Has(begin) && !Has(end))
            {
                dateQuery = $" after:{strDt} before:{endDt}";
            }
            else
            {
                dateQuery = $" sent";
                if (!Has(begin))
                {
                    dateQuery += $" after:{strDt}";
                }
                if (!Has(end))
                {
                    dateQuery += $" before:{endDt}";
                }
            }
            System.Diagnostics.Debug.WriteLine("[vertex]:GetSentQuery:dateQuery:" + dateQuery);
            return dateQuery;
        }
        protected string AppendString(string str, string str2)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            if (string.IsNullOrEmpty(str2))
            {
                return str;
            }
            if (str.Length > 0)
            {
                str += " | ";
            }
            return str + str2;
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
        public static string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            /*
            html = html.Replace("\u00A0", " ");//unicode non-breaking spaces    
            html = html.Replace("&nbsp;", " ");//html non-breaking spaces
            return Regex.Replace(html, @"<([^>]+)([^>]*?)>", delegate (Match match)
            {
                var tagName = match.Groups[1].Value.ToLower();
                if (tagName == "script" || tagName == "style")
                {
                    return ""; // Remove content of script and style tags
                }
                else
                {
                    return match.Groups[2].Value; // Keep content of other tags
                }
            });
            */
            //if using the HtmlAgilityPack third-party lib is unacceptable, we can use the code above which
            //is not as good, but only uses regular expressions. The above seems to leave a lot of styles on the page
            //but may be good enough for most emails.
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
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
        protected string ParsedShortDate(string testDate, DateTime defaultDate)
        {
            DateTime dt = defaultDate;
            System.Diagnostics.Debug.WriteLine("[vertex][ParsedShortDate]");
            if (!Has(testDate))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ParsedShortDate]testDate is empty. Using default: " + dt.ToString("yyyy/MM/dd"));
                return dt.ToString("yyyy/MM/dd");
            }
            //https://developers.google.com/gmail/api/guides/filtering
            try
            {
                dt = DateTime.Parse(testDate);
                System.Diagnostics.Debug.WriteLine("[vertex][ParsedShortDate]testDate:" + testDate);
                return dt.ToString("yyyy/MM/dd");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ParsedShortDate]ex.Message:" + ex.Message);
            }
            return dt.ToString("yyyy/MM/dd");
        }  
    }
}
