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
    public class ContactService : IContactService
    {
        private readonly ApiHelper _apiHelper;

        public ContactService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        private async Task<SalesforceContacts> QueryRawContacts(ContactsQueryRequest request, string token)
        {
            string url = "v60.0/query?q=SELECT+Id,+Name,+IsActive,+Type,+Status,+StartDate,+EndDate,+Description+FROM+Contact";
            SalesforceContacts result = await _apiHelper.Get<SalesforceContacts>(url, token);
            if (result == null || result.Records == null)
                return result;

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                result.Records = result.Records.Where(contact => contact.FirstName == request.FirstName).ToList();
            }
            if (!string.IsNullOrEmpty(request.LastName))
            {
                result.Records = result.Records.Where(contact => contact.LastName == request.LastName).ToList();
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                result.Records = result.Records.Where(contact => contact.MobilePhone == request.PhoneNumber).ToList();
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                result.Records = result.Records.Where(contact => contact.Email == request.Email).ToList();
            }
            if (!string.IsNullOrEmpty(request.Address))
            {
                result.Records = result.Records.Where(contact => contact.Address == request.Address).ToList();
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                result.Records = result.Records.Where(campaign => campaign.Description != null && campaign.Description.Contains(request.Description)).ToList();
            }

            return result;
        }

        public async Task<List<ContactResponse>> QueryContacts(ContactsQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][QueryContacts]");

                SalesforceContacts contacts = await QueryRawContacts(request, token);
                if (contacts == null || contacts.Records == null)
                    return new List<ContactResponse>();

                List<ContactResponse> result = contacts.Records.Select(contact => new ContactResponse
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumber = contact.MobilePhone,
                    Email = contact.Email,
                    Address = contact.Address,
                    Description = contact.Description
                }).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][QueryContacts]return:" + JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CreateContact(ContactCreateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][CreateContact]");

                if (string.IsNullOrEmpty(request.LastName))
                    throw new Exception("Contact lastname is not specified.");

                string url = "v60.0/sobject/Contact";
                object body = new
                {
                    request.FirstName,
                    request.LastName,
                    MobilePhone = request.PhoneNumber,
                    request.Email,
                    request.Address,
                    request.Description
                };
                bool contactCreated = await _apiHelper.Post<bool>(url, body, token);
                if (!contactCreated)
                    throw new Exception("Contact failed to create.");

                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][CreateContact]return:" + JsonConvert.SerializeObject(contactCreated));
                return contactCreated;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> UpdateContacts(ContactsUpdateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][UpdateContacts]");

                if (string.IsNullOrEmpty(request.UpdatedFirstName) && string.IsNullOrEmpty(request.UpdatedLastName) && string.IsNullOrEmpty(request.UpdatedPhoneNumber) && string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.Address) && string.IsNullOrEmpty(request.Description))
                    throw new Exception("One or more updating parameters are not specified.");

                SalesforceContacts contacts = await QueryRawContacts(new ContactsQueryRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Address = request.Address,
                    Description = request.Description
                }, token);
                if (contacts == null || contacts.Records == null)
                    throw new Exception("Contacts not found.");

                var tasks = contacts.Records.Select(async contact =>
                {
                    string urlQuery = $"v60.0/sobject/Contact/{contact.Id}";
                    object body = new
                    {
                        FirstName = string.IsNullOrEmpty(request.UpdatedFirstName) ? contact.FirstName : request.UpdatedFirstName,
                        LastName = string.IsNullOrEmpty(request.UpdatedLastName) ? contact.LastName : request.UpdatedLastName,
                        MobilePhone = string.IsNullOrEmpty(request.UpdatedPhoneNumber) ? contact.MobilePhone : request.UpdatedPhoneNumber,
                        Email = string.IsNullOrEmpty(request.UpdatedEmail) ? contact.Email : request.UpdatedEmail,
                        Address = string.IsNullOrEmpty(request.UpdatedAddress) ? contact.Address : request.UpdatedAddress,
                        Description = string.IsNullOrEmpty(request.UpdatedDescription) ? contact.Description : request.UpdatedDescription
                    };
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more contacts failed to update.");

                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][UpdateContacts]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveContacts(ContactsRemoveRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][RemoveContacts]");

                SalesforceContacts contacts = await QueryRawContacts(new ContactsQueryRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Address = request.Address,
                    Description = request.Description
                }, token);
                if (contacts == null || contacts.Records == null)
                    throw new Exception("Contacts not found.");

                var tasks = contacts.Records.Select(async contact =>
                {
                    string urlQuery = $"v60.0/sobject/Contact/{contact.Id}";
                    return await _apiHelper.Delete(urlQuery, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more contacts failed to remove.");

                System.Diagnostics.Debug.WriteLine("[vertex][ContactService][RemoveContacts]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
