using Jira.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetProjectsResponse : ServerResponse
    {
        [JsonProperty("projects"), JsonPropertyName("projects")]
        public List<JiraProject> Projects { get; set; }
    }
}
