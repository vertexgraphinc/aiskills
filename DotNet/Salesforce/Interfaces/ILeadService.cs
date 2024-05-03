using Salesforce.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Salesforce.Interfaces
{
    public interface ILeadService
    {
        Task<List<LeadResponse>> QueryLeads(LeadsQueryRequest request, string token);

        Task<bool> CreateLead(LeadsCreateRequest request, string token);

        Task<bool> UpdateLeads(LeadsUpdateRequest request, string token);

        Task<bool> RemoveLeads(LeadsQueryRequest request, string token);

    }
}
