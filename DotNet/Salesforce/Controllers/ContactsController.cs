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
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("query")]
        public async Task<ContactsQueryResponse> QueryContacts(ContactsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Query]");
            ContactsQueryResponse resp = new ContactsQueryResponse
            {
                Contacts = null
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
                resp.Contacts = await _contactService.QueryContacts(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<ServerResponse> CreateContact(ContactCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Create]");
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
                bool isCreated = await _contactService.CreateContact(request, token);
                if (isCreated)
                {
                    resp.Message = "Contact created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create contact.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateContacts(ContactsUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Update]");
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
                bool isUpdated = await _contactService.UpdateContacts(request, token);
                if (isUpdated)
                {
                    resp.Message = "Contact updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update contact.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("remove")]
        public async Task<ServerResponse> RemoveContacts(ContactsRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Remove]");
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
                bool isUpdated = await _contactService.RemoveContacts(request, token);
                if (isUpdated)
                {
                    resp.Message = "Contact removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove contact.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Contacts][Remove]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}
