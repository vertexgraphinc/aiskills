using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using MSTeams.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MSTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("query"), HttpPost("~/skill/{controller}/query")]
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

        [HttpPost("get"), HttpPost("~/skill/{controller}/get")]
        public async Task<TeamResponse> GetTeam(TeamGetRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            return await _teamService.GetTeam(request, token);
        }

        [HttpPost("create"), HttpPost("~/skill/{controller}/create")]
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

        [HttpPost("update"), HttpPost("~/skill/{controller}/update")]
        public async Task<IActionResult> UpdateTeam(TeamUpdateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isUpdated = await _teamService.UpdateTeam(request, token);
            if (isUpdated)
            {
                return Ok("Team updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update team.");
            }
        }

        [HttpPost("remove"), HttpPost("~/skill/{controller}/remove")]
        public async Task<IActionResult> RemoveTeam(TeamRemoveRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isRemoved = await _teamService.RemoveTeam(request, token);
            if (isRemoved)
            {
                return Ok("Team removed successfully.");
            }
            else
            {
                return BadRequest("Failed to remove team.");
            }
        }

        [HttpPost("queryMembers"), HttpPost("~/skill/{controller}/members/query")]
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

        [HttpPost("getMember"), HttpPost("~/skill/{controller}/members/get")]
        public async Task<MemberResponse> GetTeamMember(TeamMemberGetRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            return await _teamService.GetTeamMember(request, token);
        }

        [HttpPost("addMember"), HttpPost("~/skill/{controller}/members/add")]
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

        [HttpPost("removeMember"), HttpPost("~/skill/{controller}/members/remove")]
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
