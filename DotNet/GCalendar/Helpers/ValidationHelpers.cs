using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace GCalendar.Helpers
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
        public string AssembleOptionalParameters<T>(T parameters)
        {
            if (parameters == null)
                return string.Empty;

            var flattenedParameters = Flatten(parameters)
                .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}");

            return string.Join("&", flattenedParameters);
        }

        private Dictionary<string, string> Flatten(object obj, string prefix = "")
        {
            var result = new Dictionary<string, string>();

            if (obj == null)
                return result;

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                var value = property.GetValue(obj);

                if (value != null)
                {
                    if (IsSimpleType(property.PropertyType))
                    {
                        if (property.PropertyType == typeof(DateTime))
                        {
                            result[key] = ((DateTime) value).ToString("O");
                        }
                        else
                        {
                            result[key] = value.ToString();
                        }                    
                    }
                    else
                    {
                        var nestedDictionary = Flatten(value, key);
                        foreach (var kvp in nestedDictionary)
                        {
                            result[kvp.Key] = kvp.Value;
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime);
        }
    }
}
