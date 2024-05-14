using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetIssueTypesRequest
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
