using Newtonsoft.Json;
using Salesforce.Contracts;
using Salesforce.DTOs;
using Salesforce.Helpers;
using Salesforce.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Salesforce.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ApiHelper _apiHelper;

        public CampaignService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        private async Task<SalesforceCampaigns> QueryRawCampaigns(CampaignsQueryRequest request, string token)
        {
            string url = "v60.0/query?q=SELECT+Id,+Name,+IsActive,+Type,+Status,+StartDate,+EndDate,+Description+FROM+Campaign";
            SalesforceCampaigns result = await _apiHelper.Get<SalesforceCampaigns>(url, token);
            if (result == null || result.Records == null)
                return result;

            if (!string.IsNullOrEmpty(request.Name))
            {
                result.Records = result.Records.Where(campaign => campaign.Name == request.Name).ToList();
            }
            if (!string.IsNullOrEmpty(request.Type))
            {
                result.Records = result.Records.Where(campaign => campaign.Type == request.Type).ToList();
            }
            if (!string.IsNullOrEmpty(request.Status))
            {
                result.Records = result.Records.Where(campaign => campaign.Status == request.Status).ToList();
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                result.Records = result.Records.Where(campaign => campaign.Description != null && campaign.Description.Contains(request.Description)).ToList();
            }
            if (!string.IsNullOrEmpty(request.StartDateBeginTime))
            {
                DateTime BeginDT = DateTime.Parse(request.StartDateBeginTime).Date;
                result.Records = result.Records.Where(campaign => BeginDT >= DateTime.Parse(campaign.StartDate).Date).ToList();
            }
            if (!string.IsNullOrEmpty(request.StartDateEndTime))
            {
                DateTime EndDT = DateTime.Parse(request.StartDateEndTime);
                result.Records = result.Records.Where(campaign => EndDT <= DateTime.Parse(campaign.StartDate).Date).ToList();
            }

            return result;
        }

        public async Task<List<CampaignResponse>> QueryCampaigns(CampaignsQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][QueryCampaigns]");

                SalesforceCampaigns campaigns = await QueryRawCampaigns(request, token);
                if (campaigns == null || campaigns.Records == null)
                    return new List<CampaignResponse>();

                List<CampaignResponse> result = campaigns.Records.Select(campaign => new CampaignResponse
                {
                    Name = campaign.Name,
                    IsActive = campaign.IsActive,
                    Type = campaign.Type,
                    Status = campaign.Status,
                    StartDate = campaign.StartDate,
                    EndDate = campaign.EndDate,
                    Description = campaign.Description
                }).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][QueryCampaigns]return:" + JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CampaignResponse> CreateCampaign(CampaignCreateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][CreateCampaign]");

                if (string.IsNullOrEmpty(request.Name))
                    throw new Exception("Campaign name is not specified.");

                string url = "v60.0/sobject/Campaign";
                object body = new
                {
                    request.Name,
                    request.IsActive,
                    request.Type,
                    request.Status,
                    request.StartDate,
                    request.EndDate,
                    request.Description
                };
                SalesforceCampaign campaignCreated = await _apiHelper.Post<SalesforceCampaign>(url, body, token);
                if (campaignCreated == null || campaignCreated.Id == null)
                    throw new Exception("Campaign failed to create.");

                CampaignResponse result = new CampaignResponse
                {
                    Name = campaignCreated.Name,
                    IsActive = campaignCreated.IsActive,
                    Type = campaignCreated.Type,
                    Status = campaignCreated.Status,
                    StartDate = campaignCreated.StartDate,
                    EndDate = campaignCreated.EndDate,
                    Description = campaignCreated.Description
                };

                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][CreateCampaign]return:" + JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateCampaigns(CampaignsUpdateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][UpdateCampaigns]");

                if (string.IsNullOrEmpty(request.UpdatedName) && string.IsNullOrEmpty(request.UpdatedType) && string.IsNullOrEmpty(request.UpdatedStatus) && string.IsNullOrEmpty(request.UpdatedStartDate) && string.IsNullOrEmpty(request.UpdatedEndDate) && string.IsNullOrEmpty(request.Description))
                    throw new Exception("One or more updating parameters are not specified.");

                SalesforceCampaigns campaigns = await QueryRawCampaigns(new CampaignsQueryRequest
                {
                    Name = request.Name,
                    Type = request.Type,
                    Status = request.Status,
                    StartDateBeginTime = request.StartDateBeginTime,
                    StartDateEndTime = request.StartDateEndTime,
                    Description = request.Description
                }, token);
                if (campaigns == null || campaigns.Records == null)
                    throw new Exception("Campaigns not found.");

                var tasks = campaigns.Records.Select(async campaign =>
                {
                    string urlQuery = $"v60.0/sobject/Campaign/{campaign.Id}";
                    object body = new
                    {
                        Name = string.IsNullOrEmpty(request.UpdatedName) ? campaign.Name : request.UpdatedName,
                        Type = string.IsNullOrEmpty(request.UpdatedType) ? campaign.Type : request.UpdatedType,
                        Status = string.IsNullOrEmpty(request.UpdatedStatus) ? campaign.Status : request.UpdatedStatus,
                        StartDate = string.IsNullOrEmpty(request.UpdatedStartDate) ? campaign.StartDate : request.UpdatedStartDate,
                        EndDate = string.IsNullOrEmpty(request.UpdatedEndDate) ? campaign.EndDate : request.UpdatedEndDate,
                        Description = string.IsNullOrEmpty(request.UpdatedDescription) ? campaign.Description : request.UpdatedDescription
                    };
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more campaigns failed to update.");

                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][UpdateCampaigns]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveCampaigns(CampaignsRemoveRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][RemoveCampaigns]");

                SalesforceCampaigns campaigns = await QueryRawCampaigns(new CampaignsQueryRequest
                {
                    Name = request.Name,
                    Type = request.Type,
                    Status = request.Status,
                    StartDateBeginTime = request.StartDateBeginTime,
                    StartDateEndTime = request.StartDateEndTime,
                    Description = request.Description
                }, token);
                if (campaigns == null || campaigns.Records == null)
                    throw new Exception("Campaigns not found.");

                var tasks = campaigns.Records.Select(async campaign =>
                {
                    string urlQuery = $"v60.0/sobject/Campaign/{campaign.Id}";
                    return await _apiHelper.Delete(urlQuery, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more campaigns failed to remove.");

                System.Diagnostics.Debug.WriteLine("[vertex][CampaignService][RemoveCampaigns]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
