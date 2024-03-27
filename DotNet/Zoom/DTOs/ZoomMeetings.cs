using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Zoom.DTOs
{
    public class ZoomMeetings
    {
        [JsonProperty("next_page_token")]
        [JsonPropertyName("next_page_token")]
        public string NextPageToken { get; set; }

        [JsonProperty("page_count")]
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("page_number")]
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("page_size")]
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("total_records")]
        [JsonPropertyName("total_records")]
        public int TotalRecords { get; set; }

        [JsonProperty("meetings")]
        [JsonPropertyName("meetings")]
        public List<ZoomMeeting> Meetings { get; set; }
    }
}
