﻿using MSTeams.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSTeams.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamResponse>> QueryTeams(TeamsQueryRequest request, string token);

        Task<bool> CreateTeam(TeamCreateRequest request, string token);

        Task<bool> UpdateTeams(TeamUpdateRequest request, string token);

        Task<bool> ArchiveTeams(TeamRemoveRequest request, string token);

        Task<List<MemberResponse>> QueryTeamMembers(TeamMembersQueryRequest request, string token);

        Task<bool> AddTeamMember(TeamMemberAddRequest request, string token);

        Task<bool> RemoveTeamMember(TeamMemberRemoveRequest request, string token);
    }
}
