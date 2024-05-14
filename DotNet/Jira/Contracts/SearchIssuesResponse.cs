using Jira.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class SearchIssuesResponse : ServerResponse
    {
        [JsonProperty("issueList"), JsonPropertyName("issueList")]
        public List<SimpleJiraIssue> IssueList { get; set; }
    }
}
