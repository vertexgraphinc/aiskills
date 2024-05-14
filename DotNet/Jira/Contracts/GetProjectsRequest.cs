using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetProjectsRequest
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
