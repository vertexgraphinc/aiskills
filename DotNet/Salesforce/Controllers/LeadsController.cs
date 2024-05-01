using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Salesforce.Contracts;
using Salesforce.Helpers;
using Salesforce.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Salesforce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _LeadService;

        public LeadsController(ILeadService LeadService)
        {
            _LeadService = LeadService;
        }

        [HttpPost("query")]
        public async Task<LeadQueryResponse> QueryLeads(LeadQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Query]");
            LeadQueryResponse resp = new LeadQueryResponse
            {
                Leads = null
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
                resp.Leads = await _LeadService.QueryLeads(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<ServerResponse> CreateLead(LeadCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Create]");
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
                bool isCreated = await _LeadService.CreateLead(request, token);
                if (isCreated)
                {
                    resp.Message = "Lead created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create Lead.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateLeads(LeadsUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Update]");
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
                bool isUpdated = await _LeadService.UpdateLeads(request, token);
                if (isUpdated)
                {
                    resp.Message = "Lead updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update Lead.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("remove")]
        public async Task<ServerResponse> RemoveLeads(LeadQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Remove]");
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
                bool isUpdated = await _LeadService.RemoveLeads(request, token);
                if (isUpdated)
                {
                    resp.Message = "Lead removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove Lead.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Leads][Remove]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}
