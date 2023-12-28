using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Microsoft.DTOs
{
    public class MSGraphRecipient
    {
        [JsonPropertyName("emailAddress")]
        [JsonProperty("emailAddress")]
        public MSGraphEmailAddress EmailAddress { get; set; }
    }
}
