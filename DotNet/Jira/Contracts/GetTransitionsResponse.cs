using Jira.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class GetTransitionsResponse : ServerResponse
    {
        [JsonProperty("transitions"), JsonPropertyName("transitions")]
        public List<Transition> Transitions { get; set; }
    }
}
