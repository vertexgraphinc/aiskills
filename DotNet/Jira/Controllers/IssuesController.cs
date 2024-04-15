using Jira.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using Newtonsoft.Json;
using Jira.Helpers;
using System;

namespace Jira.Controllers
{
    [ApiController, Route("[controller]")]
    public class IssuesController : IssueHelpers
    {
        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public async Task<string> TestAsync()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages]Test");
            var test = await GetJiraSiteInfo();
            return "Issues controller test.";
        }


        #region Search Issues
        [HttpPost("search"), HttpPost("~/skill/{controller}/search")]
        public async Task<SearchIssuesResponse> SearchJiraIssues(SearchIssuesRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][GetIssues]");
            var response = new SearchIssuesResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await SearchIssues(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][GetIssues] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Get Issue 
        [HttpPost("get"), HttpPost("~/skill/{controller}/get")]
        public async Task<GetIssueResponse> GetJiraIssue(GetIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][GetIssue]");
            var response = new GetIssueResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await GetIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][GetIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Update Issue 
        [HttpPost("update"), HttpPost("~/skill/{controller}/update")]
        public async Task<ServerResponse> UdpateJiraIssue(UpdateIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][UpdateJiraIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await UpdateIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][UpdateJiraIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Comment on Issue
        [HttpPost("comment"), HttpPost("~/skill/{controller}/comment")]
        public async Task<ServerResponse> CommentJiraIssue(CommentIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][CommentJiraIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await CommentIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][CommentJiraIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Transition Issue
        [HttpPost("transition"), HttpPost("~/skill/{controller}/transition")]
        public async Task<ServerResponse> TransitionJiraIssue(TransitionIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][TransitionJiraIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await TransistionIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][TransitionJiraIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion


        #region Assign Issue
        [HttpPost("assign"), HttpPost("~/skill/{controller}/assign")]
        public async Task<ServerResponse> AssignJiraIssue(AssignIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][AssignJiraIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await AssignIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][AssignJiraIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Create Issue
        [HttpPost("create"), HttpPost("~/skill/{controller}/create")]
        public async Task<ServerResponse> CreateJiraIssue(CreateIssueRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Issues][CreateJiraIssue]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }

            try
            {
                response = await CreateIssue(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Issues][CreateJiraIssue] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion
    }
}
