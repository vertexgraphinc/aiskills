using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using MSTeams.Services;
using Newtonsoft.Json;
using System;
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
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Query]");
            TeamsQueryResponse resp = new TeamsQueryResponse
            {
                Teams = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Teams = await _teamService.QueryTeams(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<ServerResponse> CreateTeam(TeamCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Create]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isCreated = await _teamService.CreateTeam(request, token);
                if (isCreated)
                {
                    resp.Message = "Team created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create team.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateTeams(TeamUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Update]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isUpdated = await _teamService.UpdateTeams(request, token);
                if (isUpdated)
                {
                    resp.Message = "Team updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update team.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("remove")]
        public async Task<ServerResponse> RemoveTeams(TeamRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Remove]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isRemoved = await _teamService.ArchiveTeams(request, token);
                if (isRemoved)
                {
                    resp.Message = "Team removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove team.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][Remove]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/query")]
        public async Task<TeamMembersQueryResponse> QueryTeamMembers(TeamMembersQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][QueryMembers]");
            TeamMembersQueryResponse resp = new TeamMembersQueryResponse
            {
                Members = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }
            
            try
            {
                resp.Members = await _teamService.QueryTeamMembers(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][QueryMembers]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/add")]
        public async Task<ServerResponse> AddTeamMember(TeamMemberAddRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][AddMember]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isAdded = await _teamService.AddTeamMember(request, token);
                if (isAdded)
                {
                    resp.Message = "Team member added successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to add team member.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][AddMember]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/remove")]
        public async Task<ServerResponse> RemoveTeamMember(TeamMemberRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Teams][RemoveMember]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isRemoved = await _teamService.RemoveTeamMember(request, token);
                if (isRemoved)
                {
                    resp.Message = "Team member removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove team member.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Teams][RemoveMember]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}
