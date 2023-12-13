using GMail.Contracts;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace GMail.Contracts
{
    public class ValidationHelpers : ControllerBase
    {
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
        protected string GetStringVal(object val)
        {
            if (val is string)
            {
                return Sanitize(val.ToString());
            }
            return "";
        }
        protected bool IsCollection(object obj)
        {
            return obj is IEnumerable && !(obj is string);
        }
        protected string Sanitize(string str)
        {
            //remove things that can break json
            return str.Replace("\r", " ").Replace("\n", " ").Replace("\"", "''").Replace("<", " ").Replace(">", " ");
        }
        public static string StripHtmlTags(string html)
        {
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
        public static bool IsAlphaNumeric(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9]+$");
        }
        protected string ParsedShortDate(string testDate, DateTime defaultDate)
        {
            DateTime dt = defaultDate;
            //https://developers.google.com/gmail/api/guides/filtering
            try
            {
                if (!Has(testDate))
                {
                    dt = DateTime.Parse(testDate);
                    return dt.ToString("yyyy/MM/dd");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return dt.ToString("yyyy/MM/dd");
        }      
        protected string ParsedUTCDate(string testDate, DateTime defaultDate)
        {
            //Output: 2022-12-13T00:00:00Z  
            DateTime dt = defaultDate;
            DateTimeOffset utcDateTimeOffset = dt.ToUniversalTime();
            //https://developers.google.com/gmail/api/guides/filtering
            try
            {
                if (!Has(testDate))
                {
                    dt = DateTime.Parse(testDate);
                    return utcDateTimeOffset.ToString("yyyy/MM/ddTHH:mm:ssZ");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return utcDateTimeOffset.ToString("yyyy/MM/ddTHH:mm:ssZ");
        }
    }
}
