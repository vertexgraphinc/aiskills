using Jira.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using Jira.Helpers;
using System.Linq;
using Jira.Interfaces;
using System.Collections.Generic;
using Jira.DTOs;


namespace Jira.Controllers
{
    [ApiController, Route("[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public async Task<string> TestAsync()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues]Test");

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return "Unauthorized.";
            }

            var test = await _issueService.GetJiraSiteInfo(token);
            return "Issues controller test.";
        }


        #region Search Issues
        [HttpPost("search"), HttpPost("~/skill/{controller}/search")]
        public async Task<SearchIssuesResponse> SearchJiraIssues(SearchIssuesRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssues]");
            var response = new SearchIssuesResponse();
            response.IssueList = new List<SimpleJiraIssue>();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                JiraSearchIssuesResponse jiraResponse = await _issueService.SearchIssues(request, token);

                if (UtilityHelper.Has(jiraResponse.Issues))
                {
                    response.Message = "Successfully retrieved issues";

                    foreach (var issue in jiraResponse.Issues)
                    {
                        SimpleJiraIssue simplifiedIssue = _issueService.SimplifyJiraIssue(issue);
                        if (simplifiedIssue != null)
                        {
                            response.IssueList.Add(simplifiedIssue);
                        }
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to retrieve issues";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssues] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Get Issue 
        [HttpPost("get"), HttpPost("~/skill/{controller}/get")]
        public async Task<GetIssueResponse> GetJiraIssue(GetIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssue]");
            var response = new GetIssueResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                if (UtilityHelper.Has(request.IssueKey))
                {
                    JiraIssue jiraIssueResponse = await _issueService.GetIssue(request, token);

                    if (jiraIssueResponse == null || jiraIssueResponse.Key == null || jiraIssueResponse.Fields == null)
                    {
                        Response.StatusCode = 500;
                        response.Message = "Could not find issue with key";
                    } else
                    {
                        SimpleJiraIssue issueResponse = _issueService.SimplifyJiraIssue(jiraIssueResponse);
                        response.Issue = issueResponse;
                    }
                }
                else
                {
                    JiraSearchIssuesResponse jiraResponse = await _issueService.SearchIssues(request, token);
                    if (UtilityHelper.Has(jiraResponse.Issues))
                    {
                        if (jiraResponse.Issues.Count == 1)
                        {
                            response.Issue = _issueService.SimplifyJiraIssue(jiraResponse.Issues[0]);
                        }
                    }
                    else
                    {
                        Response.StatusCode = 500;
                        response.Message = "Could not find issue";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("issuetype"), HttpPost("~/skill/{controller}/issuetype")]
        public async Task<GetIssueTypesResponse> GetJiraIssueTypes(GetIssueTypesRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssueTypes]");
            var response = new GetIssueTypesResponse();
            response.Types = new List<JiraIssueType>();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response.Types = await _issueService.GetIssueTypes(token);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetIssueTypes] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("project"), HttpPost("~/skill/{controller}/project")]
        public async Task<GetProjectsResponse> GetJiraProjects(GetProjectsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetProjects]");
            var response = new GetProjectsResponse();
            response.Projects = new List<JiraProject>();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response.Projects = await _issueService.GetProjects(token);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetProjects] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        #endregion


        #region Update Issue 
        [HttpPost("update"), HttpPost("~/skill/{controller}/update")]
        public async Task<ServerResponse> UdpateJiraIssue(UpdateIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][UpdateIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            GetIssueResponse issue = await GetJiraIssue(request);
            if (Response.StatusCode == 500 || Response.StatusCode == 401)
            {
                return issue;
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);

            try
            {
                string issueTypeId = null;
                if (!string.IsNullOrEmpty(request.NewIssueType))
                {
                    issueTypeId = await _issueService.GetIssueType(request.NewIssueType, token);
                    if (string.IsNullOrEmpty(issueTypeId))
                    {
                        Response.StatusCode = 500;
                        response.Message = "Could not find Issue Type, please provide more accurate information";
                        return response;
                    }
                }

                string assigneeId = null;
                if (!string.IsNullOrEmpty(request.NewAssignee))
                {
                    assigneeId = await _issueService.FindAccountIdByName(request.NewAssignee, null, token);
                    if (string.IsNullOrEmpty(assigneeId))
                    {
                        Response.StatusCode = 500;
                        response.Message = "Could not find the Assignee, please provide more accurate information";
                        return response;
                    }
                }

                bool updated = await _issueService.UpdateIssue(issue.Issue, issueTypeId, assigneeId, request.NewSummary, request.NewDescription, token);

                if (updated)
                {
                    response.Message = "Successfully updated issue";
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to update issue";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][UpdateIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Comment on Issue
        [HttpPost("comment"), HttpPost("~/skill/{controller}/comment")]
        public async Task<ServerResponse> CommentJiraIssue(CommentIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][CommentIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            GetIssueResponse issue = await GetJiraIssue(request);
            if (Response.StatusCode == 500 || Response.StatusCode == 401)
            {
                return issue;
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);

            try
            {
                bool commented = await _issueService.CommentIssue(request, issue.Issue, token);

                if (commented)
                {
                    response.Message = "Successfully added comment";
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to add comment";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][CommentIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Transition Issue

        [HttpPost("transition"), HttpPost("~/skill/{controller}/transition")]
        public async Task<ServerResponse> TransitionJiraIssue(TransitionIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][TransitionIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            GetIssueResponse issue = await GetJiraIssue(request);
            if (Response.StatusCode == 500 || Response.StatusCode == 401)
            {
                return issue;
            }

            GetTransitionsResponse transitions = await GetJiraIssueTransitions(request);
            var transId = "";

            if (transitions.Transitions != null)
            {
                foreach (var trans in transitions.Transitions)
                {
                    if (trans.Name.ToLower() == request.Transition.ToLower())
                    {
                        transId = trans.Id;
                        break;
                    }
                }
            }

            if(string.IsNullOrEmpty(transId))
            {
                Response.StatusCode = 500;
                response.Message = "Could not find the specified transition for this issue, please provide more accurate information.";
                return response;
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);

            try
            {
                bool transitioned = await _issueService.TransistionIssue(issue.Issue, transId, token);

                if (transitioned)
                {
                    response.Message = "Successfully transitioned issue";
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to add transition issue";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][TransitionIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("transitions"), HttpPost("~/skill/{controller}/transitions")]
        public async Task<GetTransitionsResponse> GetJiraIssueTransitions(GetIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][IssueTransitions]");
            var response = new GetTransitionsResponse();
            response.Transitions = new List<Transition>();
            Response.StatusCode = 200;

            if (string.IsNullOrEmpty(request.IssueKey))
            {
                Response.StatusCode = 500;
                response.Message = "The key for the specific issue is needed to find the transitions";
                return response;
            }

            GetIssueResponse issue = await GetJiraIssue(request);
            if (Response.StatusCode == 500)
            {
                response.Message = "Could not find issue";
                return response;
            }
            else if (Response.StatusCode == 401)
            {
                response.Message = "Unauthorized.";
                return response;
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);

            try
            {
                JiraGetTransitionsResponse transitions = await _issueService.GetTransitions(issue.Issue.Key, token);

                if (transitions != null)
                {
                    response.Transitions = transitions.Transitions;
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Could not find transitions for this issue, please provide more accurate information";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][IssueTransitions] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        #endregion


        #region Assign Issue
        [HttpPost("assign"), HttpPost("~/skill/{controller}/assign")]
        public async Task<ServerResponse> AssignJiraIssue(AssignIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][AssignIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            GetIssueResponse issue = await GetJiraIssue(request);
            if (Response.StatusCode == 500 || Response.StatusCode == 401)
            {
                return issue;
            }

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);

            try
            {
                string assignId = await _issueService.FindAccountIdByName(request.NewAssigneeName, request.NewAssigneeEmail, token);

                if (string.IsNullOrEmpty(assignId))
                {
                    Response.StatusCode = 500;
                    response.Message = "Could not find assignee user, please provide more accurate information";
                    return response;
                }

                bool assigned = await _issueService.AssignIssue(issue.Issue, assignId,  token);

                if (assigned)
                {
                    response.Message = "Successfully assigned issue";
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to assign issue";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][AssignIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Create Issue
        [HttpPost("create"), HttpPost("~/skill/{controller}/create")]
        public async Task<ServerResponse> CreateJiraIssue(CreateIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][CreateIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                string issueTypeId = await _issueService.GetIssueType(request.IssueType, token);
                if(string.IsNullOrEmpty(issueTypeId))
                {
                    Response.StatusCode = 500;
                    response.Message = "Could not find Issue Type, please provide more accurate information";
                    return response;
                }

                var projectId = await _issueService.GetProjectId(request.ProjectName, token);
                if (string.IsNullOrEmpty(projectId))
                {
                    Response.StatusCode = 500;
                    response.Message = "Could not find specified project to add issue to, please provide more accurate information";
                    return response;
                }

                bool created = await _issueService.CreateIssue(request, issueTypeId, projectId, token);
                if (created)
                {
                    response.Message = "Successfully created issue";
                }
                else
                {
                    Response.StatusCode = 500;
                    response.Message = "Failed to create issue";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][CreateIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion
    }
}
