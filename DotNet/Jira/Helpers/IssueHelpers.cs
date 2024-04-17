using Newtonsoft.Json;
using Jira.Contracts;
using Jira.Helpers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;

namespace Jira.Helpers
{
    public class IssueHelpers : OAuthSession
    {

        public async Task<SearchIssuesResponse> SearchIssues(SearchIssuesRequest Para)
        {
            var result = new SearchIssuesResponse
            {
                IssueList = new List<SimpleJiraIssue>()
            };

            var query = "jql=";
          
            if (!string.IsNullOrEmpty(Para.TextQuery))
            {
                query += $"text ~\"{Para.TextQuery}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.ProjectName))
            {
                query += $"project =\"{Para.ProjectName}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.Assignee))
            {
                query += $"assignee = \"{Para.Assignee}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.Status))
            {
                query += $"status = \"{Para.Status}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.Priority))
            {
                query += $"priority = \"{Para.Priority}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.IssueType))
            {
                query += $"issuetype = \"{Para.IssueType}\" AND ";
            }

            if (query.EndsWith(" AND "))
            {
                query = query.Substring(0, query.Length - 5);
            }

            var jiraResponse = await Get<JiraSearchIssuesResponse>($"search?{query}");

            if (Has(jiraResponse.Issues))
            {

                result.Message = "Successfully retrieved issues";
                
                foreach(var issue in jiraResponse.Issues)
                {

                    result.IssueList.Add(SimplifyJiraIssue(issue));
                }
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to retrieve issues";
            }

            return result;

        }


        public async Task<GetIssueResponse> GetIssue(GetIssueRequest Para)
        {
            var result = new GetIssueResponse();
            SimpleJiraIssue issueResponse;

            if (Has(Para.IssueKey))
            {
                var jiraIssueResponse = await Get<JiraIssue>($"issue/{Para.IssueKey}");
                if (jiraIssueResponse == null || jiraIssueResponse.Key == null || jiraIssueResponse.Fields == null)
                {
                    Response.StatusCode = 500;
                    result.Message = "Could not find issue with key";
                    return result;
                }
                issueResponse = SimplifyJiraIssue(jiraIssueResponse);

            }
            else
            {
                var jiraSearchResponse = await SearchIssues(Para);
                if (jiraSearchResponse.IssueList.Count == 1)
                {
                    issueResponse = jiraSearchResponse.IssueList[0];
                }
                else
                {
                    Response.StatusCode = 500;
                    result.Message = "Could not find issue";
                    return result;
                }
            }

            result.Issue = issueResponse;
            return result;

        }

        public async Task<ServerResponse> CommentIssue(CommentIssueRequest Para)
        {
            var result = new ServerResponse();
            var searchResponse = await GetIssue(Para);

            if (Response.StatusCode == 500) {
                return searchResponse;
            }

            var issueToComment = searchResponse.Issue;

            var contentItem = new ContentItem
            {
                Content = new List<ContentItem>
            {
                new ContentItem
                {
                    Text = Para.Comment,
                    Type = "text"
                }
            },
                Type = "paragraph"
            };

            var commentObject = new JiraCommentRequest
            {
                CommentDescription = new JiraDescription {
                    Content = new List<ContentItem> { contentItem },
                    Type = "doc",
                    Version = 1
                }
            };

            var jiraResponse = await Post($"issue/{issueToComment.Key}/comment", JsonConvert.SerializeObject(commentObject, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (jiraResponse)
            {

                result.Message = "Successfully added comment";
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to add comment";
            }

            return result;

        }


        public async Task<ServerResponse> TransistionIssue(TransitionIssueRequest Para)
        {
            var result = new ServerResponse();
            var searchResponse = await GetIssue(Para);

            if (Response.StatusCode == 500)
            {
                return searchResponse;
            }

            var issueToTransition = searchResponse.Issue;


            var jiraTransitions = await Get<JiraGetTransitionsResponse>($"issue/{issueToTransition.Key}/transitions");
            var transitionID = "";

            if (jiraTransitions.Transitions != null)
            {
                
                foreach(var transition in jiraTransitions.Transitions)
                {
                    if(transition.Name.ToLower() == Para.Transition.ToLower())
                    {
                        transitionID = transition.Id; break;
                    }
                }
            }

            var transitionRequest = new JiraTransitionRequest { Transition = new JiraTransitionId { Id = transitionID } };

            var jiraResponse = await Post($"issue/{issueToTransition.Key}/transitions", JsonConvert.SerializeObject(transitionRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (jiraResponse)
            {
                result.Message = "Successfully transitioned issue";
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to add transition issue";
            }
            return result;

        }

        public async Task<ServerResponse> AssignIssue(AssignIssueRequest Para)
        {
            var result = new ServerResponse();
            var searchResponse = await GetIssue(Para);

            if (Response.StatusCode == 500)
            {
                return searchResponse;
            }

            var issueToAssign = searchResponse.Issue;


            var assigneeAccountId = await FindAccountIdByName(Para);

            if(assigneeAccountId == null)
            {
                Response.StatusCode = 500;
                result.Message = "Error finding assignee user, provide more accurate information";
                return result;
            }

            var assignIssueRequest = new JiraAssignIssueRequest { AccountId = assigneeAccountId };

            var jiraResponse = await Put($"issue/{issueToAssign.Key}/assignee", JsonConvert.SerializeObject(assignIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (jiraResponse)
            {
                result.Message = "Successfully assigned issue";
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to assign issue";
            }
            return result;

        }

        public async Task<ServerResponse> CreateIssue(CreateIssueRequest Para)
        {
            var result = new ServerResponse();

            var allIssueTypes = await Get<List<JiraIssueType>>($"issuetype");

            string issueTypeId = null;

            if (Para.IssueType != null)
            {
                foreach (JiraIssueType type in allIssueTypes)
                {
                    if (Para.IssueType.ToLower() == type.Name.ToLower())
                    {
                        issueTypeId = type.Id;
                        break;
                    }
                }
            }
            

            if(Para.ProjectKey == null && Para.ProjectName != null)
            {
                var projectSearch = await Get<JiraProjectSearchResponse>($"project/search?query={Para.ProjectName}");

                if(projectSearch.Total == 1)
                {
                    Para.ProjectKey = projectSearch.Projects[0]?.Key;
                }

            }

            
            if(Para.ProjectKey == null){
                Response.StatusCode = 500;
                result.Message = "Could not find specified project to add issue to";
                return result;
            }
            

            var assigneeId = await FindAccountIdByName(new AssignIssueRequest { NewAssigneeName = Para.Assignee });

            var createIssueRequest = new JiraCreateIssueRequest
            {
                Fields = new JiraIssueFields
                {
                    Project = new JiraProject { Key = Para.ProjectKey },
                    Summary = Para.Summary,
                    IssueType = new JiraIssueType { Id = issueTypeId },
                    Assignee = new JiraAssignee { Id = assigneeId }
                }

            };

            createIssueRequest.Fields.Description = new JiraDescription { Type = "doc", Version = 1, Content = new List<ContentItem> { new ContentItem { Type = "paragraph", Content = new List<ContentItem> { new ContentItem { Type = "text", Text = Para.Description } } } } };

            var jiraResponse = await Post($"issue", JsonConvert.SerializeObject(createIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (jiraResponse)
            {
                result.Message = "Successfully created issue";
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to create issue";
            }
            return result;

        }


        public async Task<ServerResponse> UpdateIssue(UpdateIssueRequest Para)
        {
            var result = new ServerResponse();

            var searchResponse = await GetIssue(Para);

            if (Response.StatusCode == 500)
            {
                return searchResponse;
            }

            var issueToUpdate = searchResponse.Issue;



            var updateIssueRequest = new JiraCreateIssueRequest
            {
                Fields = new JiraIssueFields()

            };

            string issueTypeId = null;

            if (!string.IsNullOrEmpty(Para.NewIssueType))
            {
                var allIssueTypes = await Get<List<JiraIssueType>>($"issuetype");
                foreach (JiraIssueType type in allIssueTypes)
                {
                    if (Para.NewIssueType.ToLower() == type.Name.ToLower())
                    {
                        issueTypeId = type.Id;
                        break;
                    }

                }

                if(issueTypeId != null)
                {
                    updateIssueRequest.Fields.IssueType = new JiraIssueType { Id = issueTypeId };
                }
            }

            if (!string.IsNullOrEmpty(Para.NewAssignee))
            {
                string assigneeId = await FindAccountIdByName(new AssignIssueRequest { NewAssigneeName = Para.NewAssignee });

                if (assigneeId != null)
                {
                    updateIssueRequest.Fields.Assignee = new JiraAssignee { Id = assigneeId };
                }
            }

            if (!string.IsNullOrEmpty(Para.NewSummary))
            {
                updateIssueRequest.Fields.Summary = Para.NewSummary;
            }

            if (!string.IsNullOrEmpty(Para.NewDescription))
            {
                updateIssueRequest.Fields.Description = new JiraDescription { Type = "doc", Version = 1, Content = new List<ContentItem> { new ContentItem { Type = "paragraph", Content = new List<ContentItem> { new ContentItem { Type = "text", Text = Para.NewDescription } } } } };
            }

            var jiraResponse = await Put($"issue/{issueToUpdate.Key}", JsonConvert.SerializeObject(updateIssueRequest, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));

            if (jiraResponse)
            {
                result.Message = "Successfully updated issue";
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = "Failed to update issue";
            }
            return result;

        }

        public async Task<string> FindAccountIdByName(AssignIssueRequest Para)
        {

            var query = "query=";

            if (!string.IsNullOrEmpty(Para.NewAssigneeName))
            {
                query += $"displayName =\"{Para.NewAssigneeName}\" AND ";
            }

            if (!string.IsNullOrEmpty(Para.NewAssigneeEmail))
            {
                query += $"emailAddress =\"{Para.NewAssigneeEmail}\" AND ";
            }

            if (query.EndsWith(" AND "))
            {
                query = query.Substring(0, query.Length - 5);
            }

            var jiraResponse = await Get<List<JiraUser>>($"user/search?{query}");

            if (jiraResponse.Count > 1 || jiraResponse.Count == 0)
            {
                return null;
            }

            return jiraResponse[0]?.AccountId;
        }

        public static SimpleJiraIssue SimplifyJiraIssue(JiraIssue jiraIssue)
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
                Description = ExtractText(jiraIssue.Fields.Description?.Content),
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

        public static string ExtractText(List<ContentItem> content)
        {
            
            string text = "";
            if (content == null) return text;
            foreach (var item in content)
            {
                if (item.Type == "text")
                {
                    text += item.Text + " ";
                }
                else 
                {
                    var newText = ExtractText(item.Content);
                    if(newText != null) {  text += newText + "\n"; }
                }
                
            }
            return text;
        }
    }
}
