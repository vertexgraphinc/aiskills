using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Zoho.DTOs;

namespace Zoho.Contracts
{
    public class CreateUpdateTicketRequest
    {
        [JsonProperty("subject"), JsonPropertyName("subject")]
        [MaxLength(255)]
        public string Subject { get; set; }

        [JsonProperty("departmentId"), JsonPropertyName("departmentId")]
        public string DepartmentId { get; set; }

        [JsonProperty("contactId"), JsonPropertyName("contactId")]
        public string ContactId { get; set; }

        [JsonProperty("productId"), JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonProperty("email"), JsonPropertyName("email")]
        [MaxLength(150)]
        public string Email { get; set; }

        [JsonProperty("phone"), JsonPropertyName("phone")]
        [MaxLength(120)]
        public string Phone { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        [MaxLength(65535)]
        public string Description { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        [MaxLength(120)]
        public string Status { get; set; }

        [JsonProperty("assigneeId"), JsonPropertyName("assigneeId")]
        public string AssigneeId { get; set; }

        [JsonProperty("category"), JsonPropertyName("category")]
        [MaxLength(300)]
        public string Category { get; set; }

        [JsonProperty("resolution"), JsonPropertyName("resolution")]
        [MaxLength(65535)]
        public string Resolution { get; set; }

        [JsonProperty("dueDate"), JsonPropertyName("dueDate")]
        public string DueDate { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        [MaxLength(120)]
        public string Priority { get; set; }

        [JsonProperty("language"), JsonPropertyName("language")]
        [MaxLength(255)]
        public string Language { get; set; }

        [JsonProperty("responseDueDate"), JsonPropertyName("responseDueDate")]
        [MaxLength(100)]
        public string ResponseDueDate { get; set; }

        [JsonProperty("channel"), JsonPropertyName("channel")]
        [MaxLength(120)]
        public string Channel { get; set; }

        [JsonProperty("classification"), JsonPropertyName("classification")]
        [MaxLength(100)]
        public string Classification { get; set; }

        [JsonProperty("teamId"), JsonPropertyName("teamId")]
        public string TeamId { get; set; }
    }

    public class TicketRequest
    {
        [JsonProperty("from"), JsonPropertyName("from")]
        public int? From { get; set; } = 0;

        [JsonProperty("limit"), JsonPropertyName("limit")]
        public int? Limit { get; set; } = 50;
    }

    public class TicketQueryRequest : TicketRequest
    {
        [JsonProperty("departmentIds"), JsonPropertyName("departmentIds")]
        public string DepartmentIds { get; set; }

        [JsonProperty("teamIds"), JsonPropertyName("teamIds")]
        public string TeamIds { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public string Assignee { get; set; }

        [JsonProperty("channel"), JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonProperty("sortBy"), JsonPropertyName("sortBy")]
        public string SortBy { get; set; }

        [JsonProperty("receivedInDays"), JsonPropertyName("receivedInDays")]
        public int? ReceivedInDays { get; set; }

        [JsonProperty("include"), JsonPropertyName("include")]
        public string Include { get; set; }

        [JsonProperty("fields"), JsonPropertyName("fields")]
        public string Fields { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public string Priority { get; set; }
    }

    public class TicketHistoryRequest : TicketRequest
    {
        [JsonProperty("eventFilter"), JsonPropertyName("eventFilter")]
        public string EventFilter { get; set; } = "";

        [JsonProperty("agentId"), JsonPropertyName("agentId")]
        public string AgentId { get; set; }

        [JsonProperty("fieldName"), JsonPropertyName("fieldName")]
        public string FieldName { get; set; } = "";
    }

    public class UpdateTicketResolutionRequest
    {
        [JsonProperty("content"), JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("isNotifyContact"), JsonPropertyName("isNotifyContact")]
        public string IsNotifyContact { get; set; }
    }

    public class TicketCommentsRequest : TicketRequest
    {
        [JsonProperty("sortBy"), JsonPropertyName("sortBy")]
        public string SortBy { get; set; }

        [JsonProperty("include"), JsonPropertyName("include")]
        public string Include { get; set; }
    }

    public class CreateCommentRequest
    {
        [JsonProperty("content"), JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonProperty("isPublic"), JsonPropertyName("isPublic")]
        public string IsPublic { get; set; }

        [JsonProperty("contentType"), JsonPropertyName("contentType")]
        public string ContentType { get; set; } = "html";
    }

}
