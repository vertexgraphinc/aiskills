﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphDateTimeTimeZone
    {
        [JsonPropertyName("dateTime")]
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("timeZone")]
        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }
}
