using Newtonsoft.Json;
using Jira.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jira.Interfaces;
using Jira.Helpers;
using Jira.Constants;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Jira.DTOs;
using System;

namespace Jira.Services
{
    public class IssueService : IIssueService
    {

        private readonly ApiHelper _apiHelper;

        public IssueService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<JiraSiteInfo> GetJiraSiteInfo(string token)
        {
            return await _apiHelper.GetJiraSiteInfo(token);
        }

        public async Task<JiraSearchIssuesResponse> SearchIssues(SearchIssuesRequest request, string token)
        {
            var query = "jql=";

            if (!string.IsNullOrEmpty(request.TextQuery))
            {
                query += $"text ~\"{request.TextQuery}\" AND ";
            }

            if (!string.IsNullOrEmpty(request.ProjectName))
            {
                query += $"project =\"{request.ProjectName}\" AND ";
            }

            if (!string.IsNullOrEmpty(request.Assignee))
            {
                query += $"assignee = \"{request.Assignee}\" AND ";
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                query += $"status = \"{request.Status}\" AND ";
            }

            if (!string.IsNullOrEmpty(request.Priority))
            {
                query += $"priority = \"{request.Priority}\" AND ";
            }

            if (!string.IsNullOrEmpty(request.IssueType))
            {
                query += $"issuetype = \"{request.IssueType}\" AND ";
            }

            if (query.EndsWith(" AND "))
            {
                query = query.Substring(0, query.Length - 5);
            }

           return await _apiHelper.Get<JiraSearchIssuesResponse>($"search?{query}", token);
        }


        public async Task<JiraIssue> GetIssue(GetIssueRequest request, string token)
        {
            return await _apiHelper.Get<JiraIssue>($"issue/{request.IssueKey}", token);
        }

        public async Task<bool> CommentIssue(CommentIssueRequest request, SimpleJiraIssue issue, string token)
        {
            var contentItem = new ContentItem
            {
                Content = new List<ContentItem>
            {
                new ContentItem
                {
                    Text = request.Comment,
                    Type = "text"
                }
            },
                Type = "paragraph"
            };

            var commentObject = new JiraCommentRequest
            {
                CommentDescription = new JiraDescription
                {
                    Content = new List<ContentItem> { contentItem },
                    Type = "doc",
                    Version = 1
                }
            };

            return await _apiHelper.Post($"issue/{issue.Key}/comment", JsonConvert.SerializeObject(commentObject, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }), token);
        }


        public async Task<bool> TransistionIssue(SimpleJiraIssue issue, string transitionId, string token)
        {
           var transitionRequest = new JiraTransitionRequest { Transition = new JiraTransitionId { Id = transitionId } };

           return await _apiHelper.Post($"issue/{issue.Key}/transitions", JsonConvert.SerializeObject(transitionRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }), token);
        }

        public async Task<bool> AssignIssue(SimpleJiraIssue issue, string assignee, string token)
        {
            var assignIssueRequest = new JiraAssignIssueRequest { AccountId = assignee };

            return await _apiHelper.Put($"issue/{issue.Key}/assignee", JsonConvert.SerializeObject(assignIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }), token);
        }

        public async Task<bool> CreateIssue(CreateIssueRequest request, string issueTypeId, string projectId, string token)
        {
            var assigneeId = await FindAccountIdByName(request.Assignee, null, token);

            var createIssueRequest = new JiraCreateIssueRequest
            {
                Fields = new JiraIssueFields
                {
                    Project = new JiraProject { Key = projectId },
                    Summary = request.Summary,
                    IssueType = new JiraIssueType { Id = issueTypeId },
                    Assignee = new JiraAssignee { Id = assigneeId }
                }
            };

            createIssueRequest.Fields.Description = new JiraDescription { Type = "doc", Version = 1, Content = new List<ContentItem> { new ContentItem { Type = "paragraph", Content = new List<ContentItem> { new ContentItem { Type = "text", Text = request.Description } } } } };

            return await _apiHelper.Post($"issue", JsonConvert.SerializeObject(createIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }), token);
        }

        public async Task<string> GetIssueType(string issueType, string token)
        {
            string issueTypeId = null;
            List<JiraIssueType> issueTypes = await GetIssueTypes(token);

            if (issueType != null && issueTypes.Count > 0)
            {
                foreach (JiraIssueType type in issueTypes)
                {
                    if (issueType.ToLower() == type.Name.ToLower())
                    {
                        issueTypeId = type.Id;
                        break;
                    }
                }
            }

            return issueTypeId;
        }

        public async Task<List<JiraIssueType>> GetIssueTypes(string token)
        {
            List<JiraIssueType> issueTypes = new List<JiraIssueType>();
            var types = await _apiHelper.Get<List<JiraIssueType>>($"issuetype", token);

            if (types != null)
            {
                issueTypes = types;
            }

            return issueTypes;
        }

        public async Task<string> GetProjectId(string projectName, string token)
        {
            string projectId = null;

            var projectSearch = await _apiHelper.Get<JiraProjectSearchResponse>($"project/search?query={projectName}", token);
            if (projectSearch != null && projectSearch.Total == 1)
            {
                projectId = projectSearch.Projects[0]?.Key;
            }

            return projectId;
        }

        public async Task<List<JiraProject>> GetProjects(string token)
        {
            List<JiraProject> projects = new List<JiraProject>();

            JiraProjectSearchResponse projectSearch = await _apiHelper.Get<JiraProjectSearchResponse>($"project/search", token);
            if (projectSearch != null)
            {
                projects = projectSearch.Projects;
            }

            return projects;
        }

        public async Task<JiraGetTransitionsResponse> GetTransitions(string issue, string token)
        {
            return await _apiHelper.Get<JiraGetTransitionsResponse>($"issue/{issue}/transitions", token);
        }


        public async Task<bool> UpdateIssue(SimpleJiraIssue issue, string issueTypeId, string assigneeId, string summary, string descript, string token)
        {
            var result = new ServerResponse();

            var updateIssueRequest = new JiraCreateIssueRequest
            {
                Fields = new JiraIssueFields()

            };

            if (!string.IsNullOrEmpty(issueTypeId))
            {
                updateIssueRequest.Fields.IssueType = new JiraIssueType { Id = issueTypeId };
            }

            if (assigneeId != null)
            {
                updateIssueRequest.Fields.Assignee = new JiraAssignee { Id = assigneeId };
            }

            if (!string.IsNullOrEmpty(summary))
            {
                updateIssueRequest.Fields.Summary = summary;
            }

            if (!string.IsNullOrEmpty(descript))
            {
                updateIssueRequest.Fields.Description = new JiraDescription { Type = "doc", Version = 1, Content = new List<ContentItem> { new ContentItem { Type = "paragraph", Content = new List<ContentItem> { new ContentItem { Type = "text", Text = descript } } } } };
            }

            return await _apiHelper.Put($"issue/{issue.Key}", JsonConvert.SerializeObject(updateIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }), token);

        }

        public async Task<string> FindAccountIdByName(string name, string email, string token)
        {
            string query = string.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                query = $"query={Uri.EscapeDataString(name)}";
            }

            else if (!string.IsNullOrEmpty(email))
            {
                query = $"query={Uri.EscapeDataString(email)}";
            }

            var jiraResponse = await _apiHelper.Get<List<JiraUser>>($"user/search?{query}", token);

            if (jiraResponse != null && jiraResponse.Count == 1)
            {
                return jiraResponse[0]?.AccountId; ;
            }

            return null;
        }

        public SimpleJiraIssue SimplifyJiraIssue(JiraIssue jiraIssue)
        {
            var issue = new SimpleJiraIssue
            {
                Key = jiraIssue.Key,
                Summary = jiraIssue.Fields.Summary,
                Status = jiraIssue.Fields.Status?.Name,
                StatusCategory = jiraIssue.Fields.Status?.StatusCategory?.Name,
                IssueType = jiraIssue.Fields.IssueType?.Name,
                ProjectName = jiraIssue.Fields.Project?.Name,
                ProjectKey = jiraIssue.Fields.Project?.Key,
                Description = UtilityHelper.ExtractText(jiraIssue.Fields.Description?.Content),
                Creator = jiraIssue.Fields.Creator?.DisplayName,
                Assignee = jiraIssue.Fields.Assignee?.DisplayName,
                Created = jiraIssue.Fields.Created,
                Updated = jiraIssue.Fields.Updated,
                Resolution = jiraIssue.Fields.Resolution?.Name,
                ResolutionDescription = jiraIssue.Fields.Resolution?.Description,
                ResolutionDate = jiraIssue.Fields.ResolutionDate,
                Priority = jiraIssue.Fields.Priority?.Name
            };

            return issue;
        }

    }
}
