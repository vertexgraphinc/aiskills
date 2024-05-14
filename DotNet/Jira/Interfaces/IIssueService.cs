using Jira.Contracts;
using Jira.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jira.Interfaces
{
    public interface IIssueService
    {
        Task<JiraSiteInfo> GetJiraSiteInfo(string token);
        Task<JiraSearchIssuesResponse> SearchIssues(SearchIssuesRequest request, string token);
        Task<JiraIssue> GetIssue(GetIssueRequest request, string token);
        Task<string> GetIssueType(string issueType, string token);
        Task<List<JiraIssueType>> GetIssueTypes(string token);
        Task<List<JiraProject>> GetProjects(string token);
        Task<JiraGetTransitionsResponse> GetTransitions(string issue, string token);
        Task<string> GetProjectId(string projectName, string token);
        Task<bool> CommentIssue(CommentIssueRequest request, SimpleJiraIssue issue, string token);
        Task<bool> TransistionIssue(SimpleJiraIssue issue, string transitionId, string token);
        Task<bool> AssignIssue(SimpleJiraIssue issue, string assignee, string token);
        Task<bool> CreateIssue(CreateIssueRequest request, string issueTypeId, string projectId, string token);
        Task<bool> UpdateIssue(SimpleJiraIssue issue, string issueTypeId, string assigneeId, string summary, string descript, string token);
        Task<string> FindAccountIdByName(string name, string email, string token);
        SimpleJiraIssue SimplifyJiraIssue(JiraIssue jiraIssue);
    }
}
