using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgentsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        [HttpGet("~/skill/{controller}")]
        public async Task<IActionResult> GetAllAgents()
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, "agents");
        }

        [HttpGet("search")]
        [HttpGet("~/skill/{controller}/search")]
        public async Task<IActionResult> QueryAgents([FromQuery] string searchStr = "")
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
            var queryString = string.IsNullOrEmpty(searchStr) ? "" : $"?searchStr={searchStr}";

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"agents{queryString}");
        }

        [HttpGet("{agent_id}")]
        [HttpGet("~/skill/{controller}/{agent_id}")]
        public async Task<IActionResult> GetAgentById(string agent_id)
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"agents/{agent_id}");
        }
    }
}
