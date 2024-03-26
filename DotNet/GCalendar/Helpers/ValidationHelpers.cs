using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using GCalendar.Contracts;

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

        public string ParseDate(string date, DateTime defaultDate, string offsetString)
        {
            DateTime dt = defaultDate;
            try
            {
                dt = DateTime.Parse(date);
               TimeSpan offset = TimeSpan.Parse(offsetString);
                DateTimeOffset dto = new DateTimeOffset(dt, offset);

                string formattedDate = dto.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
                System.Diagnostics.Debug.WriteLine("[vertex][ParseDate]date:" + date);
                System.Diagnostics.Debug.WriteLine("[vertex][ParseDate]formatted:" + formattedDate);
                return formattedDate;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ParseDate]ex.Message:" + ex.Message);
                if (dt == DateTime.MinValue) throw;
            }
            return dt.ToString("yyyy-MM-ddTHH:mm:ssZ") ;
        }

        private Dictionary<string, string> Flatten(object obj, string prefix = "")
        {
            var result = new Dictionary<string, string>();

            if (obj == null)
                return result;

            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name[..1].ToLower() + property.Name[1..];
                var key = string.IsNullOrEmpty(prefix) ? propertyName : $"{prefix}.{propertyName}";
                var value = property.GetValue(obj);

                if (value != null)
                {
                    if (IsSimpleType(property.PropertyType))
                    {
                        result[key] = value.ToString();          
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

        public SimpleEvent SimplifyEvent(Event item) {
            return new SimpleEvent
            {
                Id = item.Id,
                Status = item.Status,
                Summary = item.Summary,
                Description = item.Description,
                Location = item.Location,
                StartDateTime = Has(item.Start.DateTime) ? item.Start.DateTime : item.Start.Date,
                StartTimeZone = item.Start.TimeZone,
                EndDateTime = Has(item.End.DateTime) ? item.End.DateTime : item.End.Date,
                EndTimeZone = item.End.TimeZone,
                Recurrence = Has(item.Recurrence) ? string.Join(",", item.Recurrence) : null,
                RecurringEventId = item.RecurringEventId,
                OriginalStartDateTime = Has(item.OriginalStartTime) ? (Has(item.OriginalStartTime.DateTime) ? item.OriginalStartTime.DateTime.ToString() : item.OriginalStartTime.Date.ToString()) : null,
                OriginalStartTimeZone = Has(item.OriginalStartTime) ? item.OriginalStartTime.TimeZone : null,
                AttendeesEmails = Has(item.Recurrence) ? string.Join(",", item.Attendees.Select(attendee => attendee.Email)) : null
        };
        }
    }
}
