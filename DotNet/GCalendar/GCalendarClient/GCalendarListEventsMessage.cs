using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using GCalendar.Contracts;

namespace GCalendar.GCalendarClient
{
    public class GCalendarListEventsMessage
    {
        [JsonProperty("kind"), JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag"), JsonPropertyName("etag")]
        public string ETag { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("updated"), JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("accessRole"), JsonPropertyName("accessRole")]
        public string AccessRole { get; set; }

        [JsonProperty("defaultReminders"), JsonPropertyName("defaultReminders")]
        public List<DefaultReminder> DefaultReminders { get; set; }

        [JsonProperty("nextPageToken"), JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("nextSyncToken"), JsonPropertyName("nextSyncToken")]
        public string NextSyncToken { get; set; }

        [JsonProperty("items"), JsonPropertyName("items")]
        public List<Event> Items { get; set; }

        public class DefaultReminder
        {
            [JsonProperty("method"), JsonPropertyName("method")]
            public string Method { get; set; }

            [JsonProperty("minutes"), JsonPropertyName("minutes")]
            public int Minutes { get; set; }
        }
    }
}
