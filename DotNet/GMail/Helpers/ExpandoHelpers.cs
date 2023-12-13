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
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GMail.Helpers
{
    public class ExpandoHelpers: OAuthSession
    {
        int _defaultLookBackDays = -365;
        public string GetOptionalQStringParam(string paramName, string paramValue)
        {
            //https://support.google.com/mail/answer/7190
            //returns the formatted search operator in the form: " paramName:{paramValue}"
            if (Has(paramName) && Has(paramValue))
            {
                //                                        unlike System.Net.WebUtility.UrlEncode, Uri.EscapeDataString encodes spaces as %20 as opposed to +
                return string.Concat(" ", paramName, ":\"", Uri.EscapeDataString(paramValue), "\"");
            }
            return "";
        }
        public string GetBodyQuery(string bodyParam)
        {
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
            return dateQuery;
        }
        protected void GetProperties(dynamic expando)
        {
            if (expando is IDictionary<string, object>)
            {
                foreach (var prop in (IDictionary<string, object>)expando)
                {
                    if (IsCollection(prop.Value))
                    {
                        GetProperties(prop.Value);
                    }
                    else
                    {
                        Console.WriteLine($"prop.Key:{prop.Key} | prop.Value:{prop.Value}");
                    }
                }
            }
            else if (expando is List<Object>)
            {
                foreach (var val in (List<Object>)expando)
                {
                    if (IsCollection(val))
                    {
                        GetProperties(val);
                    }
                    else
                    {
                        Console.WriteLine($"val:{val}");
                    }
                }
            }
        }
        protected IDictionary<string, object> GetDict(dynamic expando)
        {
            if (expando is IDictionary<string, object>)
            {
                return (IDictionary<string, object>)expando;
            }
            return null;
        }
        protected List<object> GetList(dynamic expando)
        {
            if (expando is List<object>)
            {
                return (List<object>)expando;
            }
            return null;
        }
        protected string SanitizeString(string str)
        {
            //remove things that can break json
            return str.Replace("\r", " ").Replace("\n", " ").Replace("\"", "''").Replace("<", " ").Replace(">", " ");
        }
        protected bool HasAtLeastOneProp(string[] props)
        {
            if(props == null) {
                return false;
            }        
            if(props.Length == 0)   {
                return false;
            }
            foreach(string prop in props)
            {
                if (Has(prop))
                {
                    return true;
                }
            }
            return false;
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
        protected string EncodeBodyText(string body)
        {
            body = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));
            body = body.Replace('+', '-').Replace('/', '_').Replace("=", "");
            return body;
        }
        protected string ExtractDecodedTextFromParts(List<object> parts)
        {
            var sb = new StringBuilder();
            foreach (var part in parts)
            {
                string decodedText = ExtractDecodedText(part);
                if (Has(decodedText))
                {
                    sb.Append(decodedText);
                    sb.Append("\n\n");
                }
            }
            // Return an empty string or handle the case where no text content is found
            return sb.ToString();
        }
        protected string ExtractDecodedText(object part)
        {
            IDictionary<string, object> partDict = (IDictionary<string, object>)part;
            // Check if the 'Body' property exists
            if (partDict.ContainsKey("body"))
            {
                dynamic body = partDict["body"];
                var bodyDict = (IDictionary<string, object>)body;
                if (bodyDict.ContainsKey("data"))
                {
                    dynamic data = bodyDict["data"];
                    // Decode the Base64 string
                    return DecodeBase64String(data.ToString());
                }
            }
            // Return an empty string or handle the case where no text content is found
            return string.Empty;
        }
        protected string DecodeBase64String(string base64String)
        {
            byte[] data = WebEncoders.Base64UrlDecode(base64String);
            return System.Text.Encoding.UTF8.GetString(data);
        }
    }
}
