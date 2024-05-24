using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Jira.DTOs
{

    public class JiraIssue
    {
        [JsonProperty("key"), JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonProperty("fields"), JsonPropertyName("fields")]
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonProperty("status"), JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonProperty("issuetype"), JsonPropertyName("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonProperty("project"), JsonPropertyName("project")]
        public Project Project { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public Description Description { get; set; }

        [JsonProperty("creator"), JsonPropertyName("creator")]
        public User Creator { get; set; }

        [JsonProperty("assignee"), JsonPropertyName("assignee")]
        public User Assignee { get; set; }

        [JsonProperty("created"), JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonProperty("updated"), JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonProperty("resolution"), JsonPropertyName("resolution")]
        public Resolution Resolution { get; set; }

        [JsonProperty("resolutiondate"), JsonPropertyName("resolutiondate")]
        public string ResolutionDate { get; set; }

        [JsonProperty("priority"), JsonPropertyName("priority")]
        public Priority Priority { get; set; }
    }

    public class Status
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("statusCategory"), JsonPropertyName("statusCategory")]
        public StatusCategory StatusCategory { get; set; }
    }

    public class StatusCategory
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class IssueType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fields")]
        public Dictionary<string, Field> Fields { get; set; }
    }

    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("issuetypes")]
        public List<IssueType> IssueTypes { get; set; }
    }

    public class Description
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("version"), JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonProperty("content"), JsonPropertyName("content")]
        public List<ContentItem> Content { get; set; }
    }

    public class ContentItem
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonProperty("content"), JsonPropertyName("content")]
        public List<ContentItem> Content { get; set; }
    }

    public class Paragraph
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("content"), JsonPropertyName("content")]
        public List<Text> Content { get; set; }
    }

    public class Text
    {
        [JsonProperty("type"), JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("text"), JsonPropertyName("text")]
        public string Value { get; set; }
    }

    public class User
    {
        [JsonProperty("displayName"), JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }

    public class Resolution
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("description"), JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Priority
    {
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
