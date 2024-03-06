using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Slack.Contracts
{
    public class SearchFilesResponse : ServerResponse
    {
        [JsonProperty("files"), JsonPropertyName("files")]
        public FilesData Files { get; set; }

        [JsonProperty("ok"), JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonProperty("error"), JsonPropertyName("error")]
        public string Error { get; set; }
        public class FilesData
        {
            [JsonProperty("matches"), JsonPropertyName("matches")]
            public List<FileMatch> Matches { get; set; }
        }

        public class FileMatch
        {
            [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
            public string Timestamp { get; set; }

            [JsonProperty("name"), JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonProperty("user"), JsonPropertyName("user")]
            public string User { get; set; }

            [JsonProperty("size"), JsonPropertyName("size")]
            public int Size { get; set; }

            [JsonProperty("url_private_download"), JsonPropertyName("url_private_download")]
            public string UrlPrivateDownload { get; set; }

            [JsonProperty("permalink"), JsonPropertyName("permalink")]
            public string Permalink { get; set; }
        }
    }
}
