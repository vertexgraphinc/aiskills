using MSTeams.Contracts;
using MSTeams.DTOs;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSTeams.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApiHelper _apiHelper;

        public TeamService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<TeamResponse>> QueryTeams(TeamsQueryRequest request, string token)
        {
            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return new List<TeamResponse>();

                return (await Task.WhenAll(teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).Select(async team =>
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    return new TeamResponse
                    {
                        Description = team.Description,
                        DisplayName = team.DisplayName,
                        MemberEmails = string.Join(",", members.Value.Select(member => member.Email)),
                        WebUrl = team.WebUrl
                    };
                }))).Where(team =>
                {
                    if (string.IsNullOrEmpty(request.MemberEmails))
                        return true;

                    string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                    foreach (string email in emails)
                    {
                        if (!team.MemberEmails.Contains(email))
                            return false;
                    }
                    return true;
                }).ToList();
            }
            catch (HttpRequestException e)
            {
                return new List<TeamResponse>();
            }
        }

        public async Task<bool> CreateTeam(TeamCreateRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.DisplayName) || string.IsNullOrEmpty(request.MemberEmails))
                return false;

            MSGraphUserEntity creator = await _apiHelper.Get<MSGraphUserEntity>("", token);

            string query = $"teams";
            MSGraphCreateTeamBody body = new MSGraphCreateTeamBody
            {
                TemplateODataBind = "https://graph.microsoft.com/v1.0/teamsTemplates('standard')",
                Description = request.Description,
                DisplayName = request.DisplayName,
                Members = request.MemberEmails.Split(",").Select(email => new MSGraphMember
                {
                    ODataType = "#microsoft.graph.aadUserConversationMember",
                    Email = email,
                    VisibleHistoryStartDateTime = "0001-01-01T00:00:00Z",
                    Roles = new List<string>
                    {
                        "guest"
                    }
                }).Append(new MSGraphMember
                {
                    ODataType = "#microsoft.graph.aadUserConversationMember",
                    Id = creator.Id,
                    DisplayName = creator.DisplayName,
                    Email = string.IsNullOrEmpty(creator.Mail) ? creator.UserPrincipalName : creator.Mail,
                    VisibleHistoryStartDateTime = "0001-01-01T00:00:00Z",
                    Roles = new List<string>
                    {
                        "owner"
                    }
                }).ToList()
            };

            try
            {
                return await _apiHelper.Post<bool>(query, body, token);
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTeams(TeamUpdateRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.UpdatedDisplayName) && string.IsNullOrEmpty(request.UpdatedDescription))
                return false;

            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return false;

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).ToList();
                if (!filteredTeams.Any())
                    return false;

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    string memberEmails = string.Join(",", members.Value.Select(member => member.Email));

                    bool allEmailsContained = true;
                    if (!string.IsNullOrEmpty(request.MemberEmails))
                    {
                        string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                        foreach (string email in emails)
                        {
                            if (!memberEmails.Contains(email))
                            {
                                allEmailsContained = false;
                                break;
                            }
                        }
                    }

                    if (allEmailsContained)
                    {
                        string updateQuery = $"teams/{team.Id}";
                        if (!string.IsNullOrEmpty(request.UpdatedDisplayName) && !string.IsNullOrEmpty(request.UpdatedDescription))
                        {
                            object body = new
                            {
                                DisplayName = request.UpdatedDisplayName,
                                Description = request.UpdatedDescription
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        } 
                        else if (!string.IsNullOrEmpty(request.UpdatedDisplayName))
                        {
                            object body = new
                            {
                                DisplayName = request.UpdatedDisplayName
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        }
                        else
                        {
                            object body = new
                            {
                                Description = request.UpdatedDescription
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        }
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> ArchiveTeams(TeamRemoveRequest request, string token)
        {
            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return false;

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).ToList();
                if (!filteredTeams.Any())
                    return false;

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    string memberEmails = string.Join(",", members.Value.Select(member => member.Email));

                    bool allEmailsContained = true;
                    if (!string.IsNullOrEmpty(request.MemberEmails))
                    {
                        string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                        foreach (string email in emails)
                        {
                            if (!memberEmails.Contains(email))
                            {
                                allEmailsContained = false;
                                break;
                            }
                        }
                    }

                    if (allEmailsContained)
                    {
                        string archiveQuery = $"teams/{team.Id}/archive";
                        tasks.Add(_apiHelper.Post<bool>(archiveQuery, null, token));
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<List<MemberResponse>> QueryTeamMembers(TeamMembersQueryRequest request, string token)
        {
            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return new List<MemberResponse>();

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).ToList();
                if (!filteredTeams.Any())
                    return new List<MemberResponse>();

                List<Task<MSGraphMembers>> tasks = new List<Task<MSGraphMembers>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    string memberEmails = string.Join(",", members.Value.Select(member => member.Email));

                    bool allEmailsContained = true;
                    if (!string.IsNullOrEmpty(request.MemberEmails))
                    {
                        string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                        foreach (string email in emails)
                        {
                            if (!memberEmails.Contains(email))
                            {
                                allEmailsContained = false;
                                break;
                            }
                        }
                    }

                    if (allEmailsContained)
                    {
                        string searchMemberQuery = $"teams/{team.Id}/members";
                        tasks.Add(_apiHelper.Get<MSGraphMembers>(searchMemberQuery, token));
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any())
                    return new List<MemberResponse>();

                return results.SelectMany(result => result.Value.Select(member => new MemberResponse
                {
                    Email = member.Email,
                    DisplayName = member.DisplayName,
                    Roles = member.Roles,
                    VisibleHistoryStart = member.VisibleHistoryStartDateTime
                })).ToList();
            }
            catch (HttpRequestException e)
            {
                return new List<MemberResponse>();
            }
        }

        public async Task<bool> AddTeamMember(TeamMemberAddRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Email))
                return false;

            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return false;

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).ToList();
                if (!filteredTeams.Any())
                    return false;

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    string memberEmails = string.Join(",", members.Value.Select(member => member.Email));

                    bool allEmailsContained = true;
                    if (!string.IsNullOrEmpty(request.MemberEmails))
                    {
                        string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                        foreach (string email in emails)
                        {
                            if (!memberEmails.Contains(email))
                            {
                                allEmailsContained = false;
                                break;
                            }
                        }
                    }

                    if (allEmailsContained)
                    {
                        string addMemberQuery = $"teams/{team.Id}/members";
                        MSGraphMember body = new MSGraphMember
                        {
                            ODataType = "#microsoft.graph.aadUserConversationMember",
                            Email = request.Email,
                            VisibleHistoryStartDateTime = "0001-01-01T00:00:00Z",
                            Roles = new List<string>
                            {
                                "guest"
                            }
                        };
                        tasks.Add(_apiHelper.Post<bool>(addMemberQuery, body, token));
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveTeamMember(TeamMemberRemoveRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Email))
                return false;

            string searchQuery = $"joinedTeams";

            try
            {
                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return false;

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName == request.DisplayName) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description == request.Description)
                ).ToList();
                if (!filteredTeams.Any())
                    return false;

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    string memberEmails = string.Join(",", members.Value.Select(member => member.Email));

                    bool allEmailsContained = true;
                    if (!string.IsNullOrEmpty(request.MemberEmails))
                    {
                        string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                        foreach (string email in emails)
                        {
                            if (!memberEmails.Contains(email))
                            {
                                allEmailsContained = false;
                                break;
                            }
                        }
                    }

                    if (allEmailsContained)
                    {
                        List<MSGraphMember> filteredMembers = members.Value.Where(member => member.Email == request.Email).ToList();
                        if (!filteredMembers.Any())
                            return false;

                        foreach(MSGraphMember member in filteredMembers)
                        {
                            string removeMemberQuery = $"teams/{team.Id}/members/{member.Id}";
                            tasks.Add(_apiHelper.Delete(removeMemberQuery, token));
                        }
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }
    }
}
