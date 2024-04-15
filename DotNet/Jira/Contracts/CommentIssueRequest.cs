using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class CommentIssueRequest : GetIssueRequest
    {
        [JsonProperty("comment"), JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}
