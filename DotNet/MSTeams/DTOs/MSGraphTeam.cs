using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System;

namespace MSTeams.DTOs
{
    public class MSGraphTeam
    {
        [JsonPropertyName("@odata.context")]
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        [JsonPropertyName("@odata.type")]
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdDateTime")]
        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonPropertyName("displayName")]
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonPropertyName("internalId")]
        [JsonProperty("internalId")]
        public string InternalId { get; set; }

        [JsonPropertyName("classification")]
        [JsonProperty("classification")]
        public string Classification { get; set; }

        [JsonPropertyName("specialization")]
        [JsonProperty("specialization")]
        public string Specialization { get; set; }

        [JsonPropertyName("visibility")]
        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonPropertyName("webUrl")]
        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }

        [JsonPropertyName("isArchived")]
        [JsonProperty("isArchived")]
        public bool IsArchived { get; set; }

        [JsonPropertyName("tenantId")]
        [JsonProperty("tenantId")]
        public string TenantId { get; set; }
    }
}
