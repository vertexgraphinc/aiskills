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
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpPost("query")]
        public async Task<CampaignsQueryResponse> QueryCampaigns(CampaignsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Query]");
            CampaignsQueryResponse resp = new CampaignsQueryResponse
            {
                Campaigns = null
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
                resp.Campaigns = await _campaignService.QueryCampaigns(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<CampaignCreateResponse> CreateCampaign(CampaignCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Create]");
            CampaignCreateResponse resp = new CampaignCreateResponse
            {
                Campaign = null
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
                resp.Campaign = await _campaignService.CreateCampaign(request, token);
                if (resp.Campaign != null)
                {
                    resp.Message = "Campaign created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create campaign.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateCampaigns(CampaignsUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Update]");
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
                bool isUpdated = await _campaignService.UpdateCampaigns(request, token);
                if (isUpdated)
                {
                    resp.Message = "Campaign updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update campaign.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("remove")]
        public async Task<ServerResponse> RemoveCampaigns(CampaignsRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Remove]");
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
                bool isUpdated = await _campaignService.RemoveCampaigns(request, token);
                if (isUpdated)
                {
                    resp.Message = "Campaign removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove campaign.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Campaigns][Remove]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}
