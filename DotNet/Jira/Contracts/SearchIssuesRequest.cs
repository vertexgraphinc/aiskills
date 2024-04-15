using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class SearchIssuesRequest
    {
        [JsonProperty("textQuery"), JsonPropertyName("textQuery")]
        public string TextQuery { get; set; }

        [JsonProperty("projectName"), JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public string Assignee { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public string Priority { get; set; }

        [JsonProperty("issueType"), JsonPropertyName("issueType")]
        public string IssueType { get; set; }
    }
}
