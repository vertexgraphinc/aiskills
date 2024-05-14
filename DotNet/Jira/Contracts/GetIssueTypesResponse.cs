using Jira.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetIssueTypesResponse : ServerResponse
    {
        [JsonProperty("types"), JsonPropertyName("types")]
        public List<JiraIssueType> Types { get; set; }
    }
}
