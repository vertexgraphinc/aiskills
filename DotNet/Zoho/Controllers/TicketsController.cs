using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Zoho.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Zoho.Helpers;
using Zoho.DTOs;
using static Zoho.DTOs.ZohoDTOs;

namespace Zoho.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TicketsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("")]
        [HttpPost("~/skill/{controller}")]
        public async Task<IActionResult> GetTickets([FromBody] TicketQueryRequest request)
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

            List<string> validDeptIds = new List<string>();
            if (!string.IsNullOrEmpty(request.DepartmentIds))
            {
                validDeptIds = await QueryHelper.ValidateDepartmentIdsAsync(client, request.DepartmentIds);
            }

            List<string> validTeamIds = new List<string>();
            if (!string.IsNullOrEmpty(request.TeamIds))
            {
                validTeamIds = await QueryHelper.ValidateTeamIdsAsync(client, request.TeamIds);
            }

            List<string> validAssigneeIds = new List<string>();
            if (request.Assignee != null)
            {
                validAssigneeIds = await QueryHelper.ValidateAgentIdsAsync(client, request.Assignee);
            }

            List<string> validChannelIds = new List<string>();
            if (request.Channel != null)
            {
                validChannelIds = await QueryHelper.ValidateChannelIdsAsync(client, request.Channel);
            }

            var validatedSortBy = QueryHelper.ValidateSortBy(request.SortBy);

            var validatedReceivedInDays = QueryHelper.ValidateReceivedInDays(request.ReceivedInDays);

            var validatedInclude = QueryHelper.ValidateInclude(request.Include);

            var queryParams = new List<string>();

            if (request.From.HasValue) queryParams.Add($"from={request.From}");
            if (request.Limit.HasValue) queryParams.Add($"limit={request.Limit}");
            if (validDeptIds.Any()) queryParams.Add($"departmentIds={string.Join(",", validDeptIds)}");
            if (validTeamIds.Any()) queryParams.Add($"teamIds={string.Join(",", validTeamIds)}");
            if (validAssigneeIds.Any()) queryParams.Add($"assignee={string.Join(",", validAssigneeIds)}");
            if (validChannelIds.Any()) queryParams.Add($"channel={string.Join(",", validChannelIds)}");
            if (!string.IsNullOrEmpty(request.Status)) queryParams.Add($"status={request.Status}");
            if (!string.IsNullOrEmpty(validatedSortBy)) queryParams.Add($"sortBy={validatedSortBy}");
            if (validatedReceivedInDays.HasValue) queryParams.Add($"receivedInDays={validatedReceivedInDays}");
            if (!string.IsNullOrEmpty(validatedInclude)) queryParams.Add($"include={validatedInclude}");
            if (!string.IsNullOrEmpty(request.Fields)) queryParams.Add($"fields={request.Fields}");
            if (!string.IsNullOrEmpty(request.Priority)) queryParams.Add($"priority={request.Priority}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets{queryString}");
        }

        [HttpPost("create")]
        [HttpPost("~/skill/{controller}/create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateUpdateTicketRequest request)
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

            if (string.IsNullOrEmpty(request.Subject))
            {
                return BadRequest("Subject is required");
            }

            var validDeptIds = await QueryHelper.ValidateDepartmentIdsAsync(client, request.DepartmentId);

            if (validDeptIds.Count == 0)
            {
                return BadRequest("Invalid or missing Department Id");
            }
            else
            {
                request.DepartmentId = validDeptIds[0];
            }

            CreateTicket createTicket = QueryHelper.InitCreateTicket(request);

            if (!string.IsNullOrEmpty(request.ContactId))
            {
                var validContactIds = await QueryHelper.ValidateContactIdsAsync(client, request.ContactId);
                if (validContactIds.Count == 0)
                {
                    createTicket.Contact = QueryHelper.ParseContact(request.ContactId);
                    createTicket.ContactId = null;

                    if (string.IsNullOrEmpty(createTicket.Contact.LastName) && string.IsNullOrEmpty(createTicket.Contact.Email))
                    {
                        return BadRequest("Contact not found and either last Name or email is required to create a new contact");
                    }
                }
                else
                {
                    createTicket.ContactId = validContactIds[0];
                }
            }
            else
            {
                return BadRequest("The contact name or email is required to create a Ticket.");
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, settings), Encoding.UTF8, "application/json");

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Post, "tickets", jsonContent);
        }

        [HttpGet("{ticket_id}")]
        [HttpGet("~/skill/{controller}/{ticket_id}")]
        public async Task<IActionResult> GetTicketById(string ticket_id)
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
            var response = await client.GetAsync($"tickets/{ticket_id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [HttpGet("number/{ticket_number}")]
        [HttpGet("~/skill/{controller}/number/{ticket_number}")]
        public async Task<IActionResult> GetTicketByNumber(string ticket_number)
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
            var response = await client.GetAsync($"tickets/search?ticketNumber={ticket_number}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [HttpPost("{ticket_number_id}/update")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/update")]
        public async Task<IActionResult> UpdateTicket(string ticket_number_id, [FromBody] CreateUpdateTicketRequest request)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (!string.IsNullOrEmpty(request.DepartmentId))
            {
                List<string> validDeptId = await QueryHelper.ValidateDepartmentIdsAsync(client, request.DepartmentId);
                request.DepartmentId = validDeptId.Count > 0 ? validDeptId[0] : null;
            }

            if (!string.IsNullOrEmpty(request.TeamId))
            {
                List<string> validTeamId = await QueryHelper.ValidateTeamIdsAsync(client, request.TeamId);
                request.TeamId = validTeamId.Count > 0 ? validTeamId[0] : null;
            }

            if (request.AssigneeId != null)
            {
                List<string> validAssigneeId = await QueryHelper.ValidateAgentIdsAsync(client, request.AssigneeId);
                request.AssigneeId = validAssigneeId.Count > 0 ? validAssigneeId[0] : null;
            }

            if (request.Channel != null)
            {
                List<string> validChannelId = await QueryHelper.ValidateChannelIdsAsync(client, request.Channel);
                request.Channel = validChannelId.Count > 0 ? validChannelId[0] : null;
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, settings), Encoding.UTF8, "application/json");

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Patch, $"tickets/{ticket_number_id}", jsonContent);
        }

        [HttpGet("{ticket_number_id}/History")]
        [HttpGet("~/skill/{controller}/{ticket_number_id}/History")]
        public async Task<IActionResult> GetTicketHistory(string ticket_number_id, string query = "")
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
            var queryString = ApiHelper.BuildQueryString(query);

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/History{queryString}");
        }

        [HttpGet("{ticket_number_id}/resolution")]
        [HttpGet("~/skill/{controller}/{ticket_number_id}/resolution")]
        public async Task<IActionResult> GetTicketResolution(string ticket_number_id)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/resolution");
        }

        [HttpPost("{ticket_number_id}/resolution/update")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/resolution/update")]
        public async Task<IActionResult> UpdateTicketResolution(string ticket_number_id, [FromBody] UpdateTicketResolutionRequest request)
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

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, settings), Encoding.UTF8, "application/json");

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Patch, $"tickets/{ticket_number_id}/resolution", jsonContent);
        }

        [HttpPost("{ticket_number_id}/History")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/History")]
        public async Task<IActionResult> GetTicketHistory(string ticket_number_id, [FromBody] TicketHistoryRequest request)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            List<string> validAssigneeIds = new List<string>();
            if (request.AgentId != null)
            {
                validAssigneeIds = await QueryHelper.ValidateAgentIdsAsync(client, request.AgentId);
            }

            var queryParams = new List<string>();

            if (request.From.HasValue) queryParams.Add($"from={request.From}");
            if (request.Limit.HasValue) queryParams.Add($"limit={request.Limit}");
            if (!string.IsNullOrEmpty(request.EventFilter)) queryParams.Add($"eventFilter={request.EventFilter}");
            if (validAssigneeIds.Any()) queryParams.Add($"agentId={validAssigneeIds[0]}");
            if (!string.IsNullOrEmpty(request.FieldName)) queryParams.Add($"fieldName={request.FieldName}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/History{queryString}");
        }

        [HttpPost("{ticket_number_id}/comments")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/comments")]
        public async Task<IActionResult> GetTicketComments(string ticket_number_id, [FromBody] TicketCommentsRequest request)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            var validatedSortBy = QueryHelper.ValidateCommentSortBy(request.SortBy);
            var validatedInclude = QueryHelper.ValidateCommentInclude(request.Include);

            var queryParams = new List<string>();

            if (request.From.HasValue) queryParams.Add($"from={request.From}");
            if (request.Limit.HasValue) queryParams.Add($"limit={request.Limit}");
            if (!string.IsNullOrEmpty(validatedSortBy)) queryParams.Add($"sortBy={validatedSortBy}");
            if (!string.IsNullOrEmpty(validatedInclude)) queryParams.Add($"include={validatedInclude}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/comments{queryString}");
        }

        [HttpGet("{ticket_number_id}/comments/{comment_id}")]
        [HttpGet("~/skill/{controller}/{ticket_number_id}/comments/{comment_id}")]
        public async Task<IActionResult> GetTicketCommentById(string ticket_number_id, string comment_id, [FromQuery] string include)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(include))
            {
                var validatedInclude = QueryHelper.ValidateCommentInclude(include);

                if (string.IsNullOrEmpty(validatedInclude))
                {
                    queryString = $"?include={validatedInclude}";
                }
            }

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/comments/{comment_id}{queryString}");
        }

        [HttpPost("{ticket_number_id}/comments/create")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/comments/create")]
        public async Task<IActionResult> CreateTicketComment(string ticket_number_id, [FromBody] CreateCommentRequest request)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request, settings), Encoding.UTF8, "application/json");

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Post, $"tickets/{ticket_number_id}/comments", jsonContent);
        }

        [HttpPost("{ticket_number_id}/conversations")]
        [HttpPost("~/skill/{controller}/{ticket_number_id}/conversations")]
        public async Task<IActionResult> GetTicketConversations(string ticket_number_id, [FromBody] TicketRequest request)
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

            try
            {
                ticket_number_id = await QueryHelper.ValidateTicketIdAsync(client, ticket_number_id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            var queryParams = new List<string>();

            if (request.From.HasValue) queryParams.Add($"from={request.From}");
            if (request.Limit.HasValue) queryParams.Add($"limit={request.Limit}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            return await ApiHelper.SendHttpRequest(client, HttpMethod.Get, $"tickets/{ticket_number_id}/conversations{queryString}");
        }

    }
}
