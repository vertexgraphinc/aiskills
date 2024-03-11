using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MSTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("query")]
        public async Task<TeamsQueryResponse> QueryTeams(TeamsQueryRequest request)
        {
            TeamsQueryResponse resp = new TeamsQueryResponse
            {
                Teams = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.Teams = await _teamService.QueryTeams(request, token);
            return resp;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTeam(TeamCreateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isCreated = await _teamService.CreateTeam(request, token);
            if (isCreated)
            {
                return Ok("Team created successfully.");
            }
            else
            {
                return BadRequest("Failed to create team.");
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateTeams(TeamUpdateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isUpdated = await _teamService.UpdateTeams(request, token);
            if (isUpdated)
            {
                return Ok("Team updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update team.");
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveTeams(TeamRemoveRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isRemoved = await _teamService.ArchiveTeams(request, token);
            if (isRemoved)
            {
                return Ok("Team removed successfully.");
            }
            else
            {
                return BadRequest("Failed to remove team.");
            }
        }

        [HttpPost("members/query")]
        public async Task<TeamMembersQueryResponse> QueryTeamMembers(TeamMembersQueryRequest request)
        {
            TeamMembersQueryResponse resp = new TeamMembersQueryResponse
            {
                Members = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.Members = await _teamService.QueryTeamMembers(request, token);
            return resp;
        }

        [HttpPost("members/add")]
        public async Task<IActionResult> AddTeamMember(TeamMemberAddRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isAdded = await _teamService.AddTeamMember(request, token);
            if (isAdded)
            {
                return Ok("Team member added successfully.");
            }
            else
            {
                return BadRequest("Failed to add team member.");
            }
        }

        [HttpPost("members/remove")]
        public async Task<IActionResult> RemoveTeamMember(TeamMemberRemoveRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isRemoved = await _teamService.RemoveTeamMember(request, token);
            if (isRemoved)
            {
                return Ok("Team member removed successfully.");
            }
            else
            {
                return BadRequest("Failed to remove team member.");
            }
        }
    }
}
