using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.DTOs
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class JiraCreateMetaResponse
    {
        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }
    }

    public class Field
    {
        [JsonProperty("required")]
        public bool Required { get; set; }
    }

}
