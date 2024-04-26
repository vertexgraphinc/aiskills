using Salesforce.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Salesforce.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactResponse>> QueryContacts(ContactsQueryRequest request, string token);

        Task<bool> CreateContact(ContactCreateRequest request, string token);

        Task<bool> UpdateContacts(ContactsUpdateRequest request, string token);

        Task<bool> RemoveContacts(ContactsRemoveRequest request, string token);

    }
}
