﻿using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.DTOs;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using Newtonsoft.Json;
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
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][QueryTeams]");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    return new List<TeamResponse>();

                var tasks = teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).Select(async team =>
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    return new TeamResponse
                    {
                        Description = team.Description,
                        DisplayName = team.DisplayName,
                        MemberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>()),
                        WebUrl = team.WebUrl
                    };
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any())
                    throw new Exception("Team members not found.");

                List<TeamResponse> teamList = results.Where(team =>
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

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][QueryTeams]return:" + JsonConvert.SerializeObject(teamList));
                return teamList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CreateTeam(TeamCreateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][CreateTeam]");

                if (string.IsNullOrEmpty(request.MemberEmails))
                    throw new Exception("Member emails are not specified.");

                MSGraphUserEntity creator = await _apiHelper.Get<MSGraphUserEntity>("me", token);
                if (string.IsNullOrEmpty(creator.Id))
                    throw new Exception("Team creator info not found.");

                string query = $"teams";
                MSGraphCreateTeamBody body = new MSGraphCreateTeamBody
                {
                    TemplateODataBind = "https://graph.microsoft.com/v1.0/teamsTemplates('standard')",
                    Description = !string.IsNullOrEmpty(request.DisplayName) ? request.Description : "New Team",
                    DisplayName = request.DisplayName,
                    Members = new List<MemberRequestBody> { new MemberRequestBody
                    {
                        ODataType = "#microsoft.graph.aadUserConversationMember",
                        Roles = new List<string>
                        {
                            "owner"
                        },
                        UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{creator.Id}\')"
                    }}
                };
                string teamCreated = await _apiHelper.Post<string>(query, body, token);
                if (string.IsNullOrEmpty(teamCreated))
                    throw new Exception("Team failed to create.");

                string teamId = teamCreated.Replace("/teams(\'", "").Replace("\')", "");
                var tasks = request.MemberEmails.Replace(" ", "").Split(",").Select(async email => {
                    string addMemberQuery = $"teams/{teamId}/members";
                    MemberRequestBody body = new MemberRequestBody
                    {
                        ODataType = "#microsoft.graph.aadUserConversationMember",
                        Roles = new List<string>(),
                        UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{email}\')"
                    };
                    return await _apiHelper.Post<bool>(addMemberQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more team members failed to be added to newly create team.");

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][CreateTeam]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateTeams(TeamUpdateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][UpdateTeams]");

                if (string.IsNullOrEmpty(request.UpdatedDisplayName) && string.IsNullOrEmpty(request.UpdatedDescription))
                    throw new Exception("New team display name or description are not specified.");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    throw new Exception("Teams not found.");

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).ToList();
                if (!filteredTeams.Any())
                    throw new Exception("Teams not found.");

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    if (members == null || members.Value == null)
                        throw new Exception("Team members not found.");

                    string memberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>());

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
                                displayName = request.UpdatedDisplayName,
                                description = request.UpdatedDescription
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        }
                        else if (!string.IsNullOrEmpty(request.UpdatedDisplayName))
                        {
                            object body = new
                            {
                                displayName = request.UpdatedDisplayName
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        }
                        else
                        {
                            object body = new
                            {
                                description = request.UpdatedDescription
                            };
                            tasks.Add(_apiHelper.Patch(updateQuery, body, token));
                        }
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more teams failed to update.");

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][UpdateTeams]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> ArchiveTeams(TeamRemoveRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][ArchiveTeams]");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    throw new Exception("Teams not found.");

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                    (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                    (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).ToList();
                if (!filteredTeams.Any())
                    throw new Exception("Teams not found.");

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    if (members == null || members.Value == null)
                        throw new Exception("Team members not found.");

                    string memberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>());

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
                    throw new Exception("One or more teams failed to archive.");

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][ArchiveTeams]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<MemberResponse>> QueryTeamMembers(TeamMembersQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][QueryTeamMembers]");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    throw new Exception("Teams not found.");

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).ToList();
                if (!filteredTeams.Any())
                    throw new Exception("Teams not found.");

                List<MSGraphMembers> results = new List<MSGraphMembers>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    if (members == null || members.Value == null)
                        throw new Exception("Team members not found.");

                    string memberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>());

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
                        members.GroupTopic = team.DisplayName;
                        results.Add(members);
                    }
                }
                if (results == null || !results.Any())
                    return new List<MemberResponse>();

                List<MemberResponse> memberList = results.SelectMany(result => result.Value.Select(member => new MemberResponse
                {
                    Email = member.Email,
                    DisplayName = member.DisplayName,
                    Roles = member.Roles,
                    GroupTopic = result.GroupTopic,
                    VisibleHistoryStart = member.VisibleHistoryStartDateTime
                })).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][QueryTeamMembers]return:" + JsonConvert.SerializeObject(memberList));
                return memberList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> AddTeamMember(TeamMemberAddRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][AddTeamMember]");

                if (string.IsNullOrEmpty(request.Email))
                    throw new Exception("New team member email address is not specified.");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    throw new Exception("Teams not found.");

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).ToList();
                if (!filteredTeams.Any())
                    throw new Exception("Teams not found.");

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    if (members == null || members.Value == null)
                        throw new Exception("Team members not found.");

                    string memberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>());

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
                        MemberRequestBody body = new MemberRequestBody
                        {
                            ODataType = "#microsoft.graph.aadUserConversationMember",
                            Roles = new List<string>(),
                            UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{request.Email}\')"
                        };
                        tasks.Add(_apiHelper.Post<bool>(addMemberQuery, body, token));
                    }
                }
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more team members failed to be added.");

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][AddTeamMember]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveTeamMember(TeamMemberRemoveRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][RemoveTeamMember]");

                if (string.IsNullOrEmpty(request.Email))
                    throw new Exception("Team member email address is not specified.");

                string searchQuery = $"me/joinedTeams";

                MSGraphTeams teams = await _apiHelper.Get<MSGraphTeams>(searchQuery, token);
                if (teams == null || teams.Value == null)
                    throw new Exception("Teams not found.");

                List<MSGraphTeam> filteredTeams = teams.Value.Where(team =>
                   (string.IsNullOrEmpty(request.DisplayName) || team.DisplayName.Contains(request.DisplayName)) &&
                   (string.IsNullOrEmpty(request.Description) || team.Description.Contains(request.Description))
                ).ToList();
                if (!filteredTeams.Any())
                    throw new Exception("Teams not found.");

                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (MSGraphTeam team in filteredTeams)
                {
                    string memberQuery = $"teams/{team.Id}/members";
                    MSGraphMembers members = await _apiHelper.Get<MSGraphMembers>(memberQuery, token);
                    if (members == null || members.Value == null)
                        throw new Exception("Team members not found.");

                    string memberEmails = string.Join(",", (members != null && members.Value != null) ? members.Value.Select(member => member.Email) : new List<string>());

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
                    throw new Exception("One or more team members failed to remove.");

                System.Diagnostics.Debug.WriteLine("[vertex][TeamService][RemoveTeamMember]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
