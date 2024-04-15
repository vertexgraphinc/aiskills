using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetIssueRequest : SearchIssuesRequest
    {
        [JsonProperty("issueKey"), JsonPropertyName("issueKey")]
        public string IssueKey { get; set; }
    }
}
