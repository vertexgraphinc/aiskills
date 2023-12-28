using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Microsoft.DTOs
{
    public class MSGraphMailFolder
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("parentFolderId")]
        [JsonProperty("parentFolderId")]
        public string ParentFolderId { get; set; }

        [JsonPropertyName("childFolderCount")]
        [JsonProperty("childFolderCount")]
        public int ChildFolderCount { get; set; }

        [JsonPropertyName("unreadItemCount")]
        [JsonProperty("unreadItemCount")]
        public int UnreadItemCount { get; set; }

        [JsonPropertyName("totalItemCount")]
        [JsonProperty("totalItemCount")]
        public int TotalItemCount { get; set; }

        [JsonPropertyName("isHidden")]
        [JsonProperty("isHidden")]
        public bool? IsHidden { get; set; }
    }
}
