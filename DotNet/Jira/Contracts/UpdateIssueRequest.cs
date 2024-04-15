using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class UpdateIssueRequest : GetIssueRequest
    {
        [JsonProperty("newSummary"), JsonPropertyName("newSummary")]
        public string NewSummary { get; set; }

        [JsonProperty("newDescription"), JsonPropertyName("newDescription")]
        public string NewDescription { get; set; }

        [JsonProperty("newIssuetype"), JsonPropertyName("newIssuetype")]
        public string NewIssueType { get; set; }

        [JsonProperty("newAssignee"), JsonPropertyName("newAssignee")]
        public string NewAssignee { get; set; }
    }
}
