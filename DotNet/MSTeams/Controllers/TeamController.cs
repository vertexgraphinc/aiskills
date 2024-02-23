using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Interfaces;
using System.Threading.Tasks;

namespace MSTeams.Controllers
{
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("query")]
        public async Task<TeamsQueryResponse> QueryTeams(TeamsQueryRequest request)
        {

        }

        [HttpPost("get")]
        public async Task<TeamResponse> GetTeam(TeamGetRequest request)
        {

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTeam(TeamCreateRequest request)
        {

        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTeam(TeamUpdateRequest request)
        {

        }

        [HttpPost("queryMembers")]
        public async Task<TeamMembersQueryResponse> QueryTeamMembers(TeamMembersQueryRequest request)
        {

        }

        [HttpPost("getMember")]
        public async Task<MemberResponse> GetTeamMember(TeamMemberGetRequest request)
        {

        }

        [HttpPost("addMember")]
        public async Task<IActionResult> AddTeamMember(TeamMemberAddRequest request)
        {

        }

        [HttpPost("removeMember")]
        public async Task<IActionResult> RemoveTeamMember(TeamMemberRemoveRequest request)
        {

        }
    }
}
