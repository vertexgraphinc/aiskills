using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Zoho.Contracts;
using System.Collections.Generic;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("")]
        [HttpPost("~/skill/{controller}")]
        public async Task<IActionResult> GetAllContacts([FromBody] TicketRequest request)
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

            var queryParams = new List<string>();

            if (request.From.HasValue) queryParams.Add($"from={request.From}");
            if (request.Limit.HasValue) queryParams.Add($"limit={request.Limit}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;


            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"contacts{queryString}");
        }

        [HttpGet("search")]
        [HttpGet("~/skill/{controller}/search")]
        public async Task<IActionResult> QueryContacts([FromQuery] string searchStr = "")
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
            var queryString = string.IsNullOrEmpty(searchStr) ? "" : $"?_all={searchStr}";

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"contacts/search{queryString}");
        }

        [HttpGet("{contact_id}")]
        [HttpGet("~/skill/{controller}/{contact_id}")]
        public async Task<IActionResult> GetContactById(string contact_id)
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

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"contacts/{contact_id}");
        }
    }
}
