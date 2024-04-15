using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetIssueResponse : ServerResponse
    {
        [JsonProperty("issue"), JsonPropertyName("issue")]
        public SimpleJiraIssue Issue { get; set; }
    }
}
