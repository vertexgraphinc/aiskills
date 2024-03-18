using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphMessages
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public List<MSGraphMessage> Values { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        [JsonProperty("@odata.nextLink")]
        public string ODataNextLink { get; set; }
    }
}
