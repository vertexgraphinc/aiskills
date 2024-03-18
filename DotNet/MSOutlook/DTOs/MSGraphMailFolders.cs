using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace MSOutlook.DTOs
{
    public class MSGraphMailFolders
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("value")]
        [JsonProperty("value")]
        public List<MSGraphMailFolder> Value { get; set; }
    }
}
