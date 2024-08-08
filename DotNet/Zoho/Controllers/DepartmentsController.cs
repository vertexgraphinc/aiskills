using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        [HttpGet("~/skill/{controller}/")]
        public async Task<IActionResult> GetDepartments()
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, "departments");
        }

        [HttpGet("query")]
        [HttpGet("~/skill/{controller}/query")]
        public async Task<IActionResult> QueryDepartments([FromQuery] string searchStr = "")
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"departments{queryString}");
        }

        [HttpGet("{department_id}")]
        [HttpGet("~/skill/{controller}/{department_id}")]
        public async Task<IActionResult> GetDepartmentById(string department_id)
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"departments/{department_id}");
        }
    }
}
