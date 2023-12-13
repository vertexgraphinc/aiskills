using GMail.Contracts;
using GMail.Helpers;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : MessagesHelpers 
    {
        [HttpPost("query")]
        public async Task<QueryEmailsResponse> QueryEmails(SearchFilters Para, bool ReturnfullMessages = true)
        {
            var resp = new QueryEmailsResponse
            {
                EmailMessages = null
            };

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                resp.MessageFromServer = "Unauthorized.";
            }
            try
            {
                resp.EmailMessages = await ListMessages(Para, ReturnfullMessages);
                if (resp.EmailMessages.Count == 0)
                {
                    resp.MessageFromServer = "No emails were found.";
                }
                else
                {
                    resp.MessageFromServer = $"Found {resp.EmailMessages.Count} email(s).";
                }
            }
            catch(Exception ex)
            {
                resp.MessageFromServer = ex.Message;
            }
            return resp;
        }
        [HttpPost("send")]
        public async Task<ServerResponse> SendEmail(SendEmailRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var resp = await SendMessage(Para);
                if (resp != null && resp.MessageFromServer == "")
                {
                    response.MessageFromServer = "Success";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("reply")]
        protected async Task<ServerResponse> ReplyEmail(ReplyToEmailRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var resp = await ReplyToEmail(Para);
                if (resp != null && resp.MessageFromServer == "")
                {
                    response.MessageFromServer = "Success";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("query_reply")]
        public async Task<ServerResponse> QueryAndReplyToEmails(QueryAndReplyToEmailsRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var SearchPara = new SearchFilters();
                SearchPara.From = Para.From;
                SearchPara.To = Para.To;
                SearchPara.Subject = Para.Subject;
                SearchPara.Body = Para.Body;
                SearchPara.BeginTime = Para.BeginTime;
                SearchPara.EndTime = Para.EndTime;
                SearchPara.Label = Para.Label;
                SearchPara.Status = Para.Status;
                int success = 0;
                List<GMailMessage> emailsFound = await ListMessages(SearchPara, false);
                if (emailsFound != null && emailsFound.Count > 0)
                {
                    foreach (var email in emailsFound)
                    {
                        var ReplyPara = new ReplyToEmailRequest();
                        ReplyPara.Id = email.Id;
                        ReplyPara.NewSubject = Para.NewSubject;
                        ReplyPara.NewBody = Para.NewBody;
                        var resp = await ReplyToEmail(ReplyPara);
                        if(resp != null && resp.MessageFromServer == "")
                        {
                            success++;
                        }
                    }
                }
                else
                {
                    response.MessageFromServer = "No emails were found.";
                }
                if(success > 0)
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s). Successfully replied to {success} email(s).";
                }
                else
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s), but the reply was not sent.";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("foward")]
        public async Task<ServerResponse> ForwardEmail(ForwardEmailRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var resp = await ForwardEmail(Para);
                if(resp != null && resp.MessageFromServer == "")
                {
                    response.MessageFromServer = "Success";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("query_foward")]
        public async Task<ServerResponse> QueryAndForwardEmails(QueryAndForwardEmailsRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var SearchPara = new SearchFilters();
                SearchPara.From = Para.From;
                SearchPara.To = Para.To;
                SearchPara.Subject = Para.Subject;
                SearchPara.Body = Para.Body;
                SearchPara.BeginTime = Para.BeginTime;
                SearchPara.EndTime = Para.EndTime;
                SearchPara.Label = Para.Label;
                SearchPara.Status = Para.Status;
                int success = 0;
                List<GMailMessage> emailsFound = await ListMessages(SearchPara, false);
                if (emailsFound != null && emailsFound.Count > 0)
                {
                    foreach (var email in emailsFound)
                    {
                        var ForwardPara = new ForwardEmailRequest();
                        ForwardPara.Id = email.Id;
                        ForwardPara.NewTo = Para.NewTo;
                        ForwardPara.NewCC = Para.NewCC;
                        ForwardPara.NewBCC = Para.NewBCC;
                        var resp = await ForwardEmail(ForwardPara);
                        if (resp != null && resp.MessageFromServer == "")
                        {
                            success++;
                        }
                    }
                }
                else
                {
                    response.MessageFromServer = "No emails were found.";
                }
                if (success > 0)
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s). Successfully forwarded {success} email(s).";
                }
                else
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s), but no emails were forwarded.";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }  
        [HttpPost("add_label")]
        public async Task<ServerResponse> AddLabel(AddLabelToMessageRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var resp = await AddLabelToMessage(Para);
                if(resp != null && resp.MessageFromServer == "")
                {
                    response.MessageFromServer = "Success";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("query_add_label")]
        public async Task<ServerResponse> QueryAndAddLabelToEmails(QueryAndAddLabelToEmailsRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var SearchPara = new SearchFilters();
                SearchPara.From = Para.From;
                SearchPara.To = Para.To;
                SearchPara.Subject = Para.Subject;
                SearchPara.Body = Para.Body;
                SearchPara.BeginTime = Para.BeginTime;
                SearchPara.EndTime = Para.EndTime;
                SearchPara.Label = Para.Label;
                SearchPara.Status = Para.Status;
                int success = 0;
                List<GMailMessage> emailsFound = await ListMessages(SearchPara, false);
                if (emailsFound != null && emailsFound.Count > 0)
                {
                    foreach (var email in emailsFound)
                    {
                        var AddLabelPara = new AddLabelToMessageRequest();
                        AddLabelPara.Id = email.Id;
                        AddLabelPara.AddLabel = Para.AddLabel;
                        var resp = await AddLabel(AddLabelPara);
                        if (resp != null && resp.MessageFromServer == "")
                        {
                            success++;
                        }
                    }
                }
                else
                {
                    response.MessageFromServer = "No emails were found.";
                }
                if (success > 0)
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s). Successfully added the label to {success} email(s).";
                }
                else
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s), but the label was not added.";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }   
        [HttpPost("remove_label")]
        public async Task<ServerResponse> RemoveLabel(RemoveLabelFromMessageRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var resp = await RemoveLabelFromMessage(Para);
                if (resp != null && resp.MessageFromServer == "")
                {
                    response.MessageFromServer = "Success";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        [HttpPost("query_remove_label")]
        public async Task<ServerResponse> QueryAndRemoveLabelFromEmails(QueryAndRemoveLabelFromEmailsRequest Para)
        {
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.MessageFromServer = "Unauthorized.";
            }
            try
            {
                var SearchPara = new SearchFilters();
                SearchPara.From = Para.From;
                SearchPara.To = Para.To;
                SearchPara.Subject = Para.Subject;
                SearchPara.Body = Para.Body;
                SearchPara.BeginTime = Para.BeginTime;
                SearchPara.EndTime = Para.EndTime;
                SearchPara.Label = Para.Label;
                SearchPara.Status = Para.Status;
                int success = 0;
                List<GMailMessage> emailsFound = await ListMessages(SearchPara, false);
                if (emailsFound != null && emailsFound.Count > 0)
                {
                    foreach (var email in emailsFound)
                    {
                        var RemoveLabelPara = new RemoveLabelFromMessageRequest();
                        RemoveLabelPara.Id = email.Id;
                        RemoveLabelPara.RemoveLabel = Para.RemoveLabel;
                        var resp = await RemoveLabel(RemoveLabelPara);
                        if (resp != null && resp.MessageFromServer == "")
                        {
                            success++;
                        }
                    }
                }
                else
                {
                    response.MessageFromServer = "No emails were found.";
                }
                if (success > 0)
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s). Successfully removed the label from {success} email(s).";
                }
                else
                {
                    response.MessageFromServer = $"Found {emailsFound.Count} email(s), but the label was not removed.";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.MessageFromServer = ex.Message;
            }
            return response;
        }   
     
    }
}
