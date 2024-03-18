using MSOutlook.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using System;

namespace MSOutlook.Helpers
{
    public class JsonHelper
    {
        public static bool IsJsonObject(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (System.Text.Json.JsonException)
            {
                return false;
            }
        }

        public static List<EmailResponse> GetEmailsResponseJsonObject(string jsonString)
        {
            var msgs = new List<EmailResponse>();

            try
            {
                var result = JsonConvert.DeserializeObject<QueryEmailsResponse>(jsonString);
                foreach (var msg in result.EmailMessages)
                {
                    msgs.Add(msg);
                }
            }
            catch (Exception)
            {
                var result = JsonConvert.DeserializeObject<EmailResponse>(jsonString);

            }

            return msgs;
        }

        public static bool IsJsonArray(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            jsonString = jsonString.Trim();
            return jsonString.StartsWith("[") && jsonString.EndsWith("]");
        }
    }
}
