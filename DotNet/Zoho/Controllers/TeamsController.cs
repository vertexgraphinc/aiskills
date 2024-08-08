using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        [HttpGet("~/skill/{controller}/")]
        public async Task<IActionResult> GetTeams()
        {
            string authorizationHeader;
            try
            {
                authorizationHeader = ApiHelper.GetAuthorizationHeader(_httpContextAccessor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

            var client = ApiHelper.CreateHttpClient(_httpClientFactory, authorizationHeader);

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"teams");
        }

        [HttpGet("{team_id}")]
        [HttpGet("~/skill/{controller}/{team_id}")]
        public async Task<IActionResult> GetTeamId(string team_id)
        {
            string authorizationHeader;
            try
            {
                authorizationHeader = ApiHelper.GetAuthorizationHeader(_httpContextAccessor);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

            var client = ApiHelper.CreateHttpClient(_httpClientFactory, authorizationHeader);

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"teams/{team_id}");
        }
    }
}
