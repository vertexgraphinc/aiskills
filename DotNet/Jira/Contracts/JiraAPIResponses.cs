using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class JiraSiteInfo
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("url"), JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonProperty("scopes"), JsonPropertyName("scopes")]
        public string[] Scopes { get; set; }

        [JsonProperty("avatarUrl"), JsonPropertyName("avatarUrl")]
        public string AvatarUrl { get; set; }
    }

    public class JiraSearchIssuesResponse
    {
        [JsonProperty("total"), JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonProperty("issues"), JsonPropertyName("issues")]
        public List<JiraIssue> Issues { get; set; }
    }


    public class JiraGetTransitionsResponse
    {
        [JsonProperty("transitions"), JsonPropertyName("transitions")]
        public List<Transition> Transitions { get; set; }
    }

    public class JiraTransitionRequest
    {
        [JsonProperty("transition"), JsonPropertyName("transition")]
        public JiraTransitionId Transition { get; set; }
    }

    public class JiraTransitionId
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }
    }


    public class Transition
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("to"), JsonPropertyName("to")]
        public TrasitionStatus ToStatus { get; set; }

        [JsonProperty("isAvailable"), JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }
    }

    public class TrasitionStatus
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("statusCategory"), JsonPropertyName("statusCategory")]
        public TransitionStatusCategory StatusCategory { get; set; }
    }

    public class TransitionStatusCategory
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("key"), JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonProperty("colorName"), JsonPropertyName("colorName")]
        public string ColorName { get; set; }
    }

    public class JiraUser
    {
        [JsonProperty("self"), JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonProperty("accountId"), JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("accountType"), JsonPropertyName("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("emailAddress"), JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("active"), JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonProperty("timeZone"), JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("locale"), JsonPropertyName("locale")]
        public string Locale { get; set; }
    }

    public class JiraAssignIssueRequest
    {
        [JsonProperty("accountId"), JsonPropertyName("accountId")]
        public string AccountId { get; set; }
    }


    public class JiraCreateIssueRequest
    {
        [JsonProperty("fields"), JsonPropertyName("fields")]
        public JiraIssueFields Fields { get; set; }

       
    }

    public class JiraDescription
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("content")]
        public List<ContentItem> Content { get; set; }
    }

    public class JiraContentItem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("content")]
        public List<ContentItem> Content { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class JiraIssueFields
    {
        [JsonProperty("project"), JsonPropertyName("project")]
        public JiraProject Project { get; set; }

        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public JiraDescription Description { get; set; }

        [JsonProperty("issuetype"), JsonPropertyName("issuetype")]
        public JiraIssueType IssueType { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public JiraPriority Priority { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public JiraAssignee Assignee { get; set; }

    }

    public class JiraIssueType
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class JiraProject
    {
        [JsonProperty("key"), JsonPropertyName("key")]
        public string Key { get; set; }
    }
    public class JiraPriority
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class JiraAssignee
    {
        [JsonProperty("id"), JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class JiraSearchPriorityResponse
    {
        [JsonProperty("total"), JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonProperty("values"), JsonPropertyName("values")]
        public List<JiraPriority> AllPriorities { get; set; }
    }


    public class JiraCommentRequest
    {
        [JsonProperty("body"), JsonPropertyName("body")]
        public JiraDescription CommentDescription { get; set; }

    }

}
