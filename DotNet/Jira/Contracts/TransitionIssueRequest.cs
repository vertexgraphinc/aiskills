using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class TransitionIssueRequest : GetIssueRequest
    {
        [JsonProperty("transition"), JsonPropertyName("transition")]
        public string Transition { get; set; }
    }
}
