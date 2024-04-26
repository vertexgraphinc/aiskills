using Salesforce.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Salesforce.Interfaces
{
    public interface ICampaignService
    {
        Task<List<CampaignResponse>> QueryCampaigns(CampaignsQueryRequest request, string token);

        Task<CampaignResponse> CreateCampaign(CampaignCreateRequest request, string token);

        Task<bool> UpdateCampaigns(CampaignsUpdateRequest request, string token);

        Task<bool> RemoveCampaigns(CampaignsRemoveRequest request, string token);

    }
}
