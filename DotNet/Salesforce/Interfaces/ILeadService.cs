using Salesforce.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Salesforce.Interfaces
{
    public interface ILeadService
    {
        Task<List<LeadResponse>> QueryLeads(LeadQueryRequest request, string token);

        Task<bool> CreateLead(LeadCreateRequest request, string token);

        Task<bool> UpdateLeads(LeadsUpdateRequest request, string token);

        Task<bool> RemoveLeads(LeadQueryRequest request, string token);

    }
}
