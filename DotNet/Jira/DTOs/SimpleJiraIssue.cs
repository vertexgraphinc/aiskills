using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace Jira.DTOs
{

    public class SimpleJiraIssue
    {
        [JsonProperty("key"), JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("statusCategory"), JsonPropertyName("statusCategory")]
        public string StatusCategory { get; set; }

        [JsonProperty("issueType"), JsonPropertyName("issueType")]
        public string IssueType { get; set; }

        [JsonProperty("projectName"), JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("projectKey"), JsonPropertyName("projectKey")]
        public string ProjectKey { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("creator"), JsonPropertyName("creator")]
        public string Creator { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public string Assignee { get; set; }

        [JsonProperty("created"), JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonProperty("updated"), JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonProperty("resolution"), JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonProperty("resolutionDescription"), JsonPropertyName("resolutionDescription")]
        public string ResolutionDescription { get; set; }

        [JsonProperty("resolutionDate"), JsonPropertyName("resolutionDate")]
        public string ResolutionDate { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public string Priority { get; set; }
    }
}
