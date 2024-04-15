using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class CreateIssueRequest
    {

        [JsonProperty("projectKey"), JsonPropertyName("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("projectName"), JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("issuetype"), JsonPropertyName("issuetype")]
        public string IssueType { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public string Priority { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public string Assignee { get; set; }
    }
}
