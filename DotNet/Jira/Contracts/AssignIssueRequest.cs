using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class AssignIssueRequest : GetIssueRequest
    {
        [JsonProperty("newAssigneeName"), JsonPropertyName("newAssigneeName")]
        public string NewAssigneeName { get; set; }

        [JsonProperty("newAssigneeEmail"), JsonPropertyName("newAssigneeEmail")]
        public string NewAssigneeEmail { get; set; }
    }
}
