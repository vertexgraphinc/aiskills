using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System.Collections.Generic;
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

        }

        public async Task<bool> CreateTeam(TeamCreateRequest request, string token)
        {

        }

        public async Task<bool> UpdateTeams(TeamUpdateRequest request, string token)
        {

        }

        public async Task<bool> RemoveTeams(TeamRemoveRequest request, string token)
        {

        }

        public async Task<List<MemberResponse>> QueryTeamMembers(TeamMembersQueryRequest request, string token)
        {

        }

        public async Task<bool> AddTeamMember(TeamMemberAddRequest request, string token)
        {

        }

        public async Task<bool> RemoveTeamMember(TeamMemberRemoveRequest request, string token)
        {

        }
    }
}
