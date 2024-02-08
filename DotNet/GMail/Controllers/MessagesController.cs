using GMail.Contracts;
using GMail.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GMail.Controllers
{
    [ApiController,Route("[controller]")]
    public class MessagesController : MessagesHelpers
    {
        [HttpGet("test"),HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "gmail" in IIS, then both URLs will work:
            //https://example.com/gmail/messages/test
            //https://example.com/gmail/skill/messages/test
            System.Diagnostics.Debug.WriteLine("[vertex][Messages]Test");
            return "hello world from messages.";
        }

        #region Getting Emails
        [HttpPost("query"),HttpPost("~/skill/{controller}/query")]
        public async Task<QueryEmailsResponse> QueryEmails(SearchFilters Para)
        {
            //for testing the route, try a rest api post with:
            //url: https://example.com/gmail/messages/query
            //header
            //     Content-Type: application/json
            //request body payload
            //     {"from":"test"} 
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][QueryEmails]");
            var resp = new QueryEmailsResponse
            {
                Messages = null
            };

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }
            try
            {
                resp.Messages = await ListMessages(Para);
                if (resp.Messages.Count == 0)
                {
                    resp.Message = "No emails were found.";
                }
                else
                {
                    resp.Message = $"Found {resp.Messages.Count} email(s).";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                resp.Message = Sanitize(StripHtmlTags( ex.ToString()));
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][QueryEmails]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("get"),HttpPost("~/skill/{controller}/get")]
        public async Task<GetEmailsResponse> GetEmail(SearchFilters Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][GetEmail]");
            var response = new GetEmailsResponse();
            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            var RetMsgs = new List<GMailMessage>();
            Response.StatusCode = 200;

            QueryEmailsResponse emails = await QueryEmails(Para);
            if (emails != null && emails.Messages != null && emails.Messages.Count  > 0)
            {
                try
                {
                    foreach(var eml in emails.Messages)
                    {
                        GetEmailRequest msgPara = new GetEmailRequest();
                        msgPara.Id = eml.Id;
                        var resp = await GetMessage(msgPara);
                        if (resp == null)
                        {
                        }
                        else
                        {
                            RetMsgs.Add(resp);
                            response.Message = "Success";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    response.Message = ex.Message;
                    return response;
                }
                response.Messages = RetMsgs;
            }
            System.Diagnostics.Debug.WriteLine("[vertex][GetEmail]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Sending Emails
        [HttpPost("send"),HttpPost("~/skill/{controller}/send")]
        public async Task<ServerResponse> SendEmail(SendEmailRequest Para)
        {
            //EXAMPLE PROMPT: send an email to user@example.com with subject "hello" and body "hello world"
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendEmail]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }
            try
            {
                var smResp = await SendMessage(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                resp.Message = Sanitize(StripHtmlTags(ex.ToString()));
                return resp;
            }
            if (!Has(resp.Message))
            {
                resp.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendEmail]response:" + JsonConvert.SerializeObject(resp));

            return resp;
        }

        [HttpPost("forward"),HttpPost("~/skill/{controller}/forward")]
        public async Task<ServerResponse> ForwardEmail(ForwardEmailRequest Para)
        {
            //according to the rfc822 standard:
            /* https://datatracker.ietf.org/doc/html/rfc2822#appendix-A.2
               ""Forwarding" has two meanings: One sense of forwarding is that a mail
               reading program can be told by a user to forward a copy of a message
               to another person, making the forwarded message the body of the new
               message.  A forwarded message in this sense does not appear to have
               come from the original sender, but is an entirely new message from
               the forwarder of the message.  On the other hand, forwarding is also
               used to mean when a mail transport program gets a message and
               forwards it on to a different destination for final delivery.  Resent
               header fields are not intended for use with either type of
               forwarding." 
             */
            //EXAMPLE PROMPT: search for an email containing XYZ in the subject and forward it to user@example.com
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ForwardEmail]");
            var response = new GetEmailsResponse();
            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            Response.StatusCode = 200;
            QueryEmailsResponse emails = await QueryEmails(Para);
            if (emails != null && emails.Messages != null && emails.Messages.Count > 0)
            {
                try
                {
                    string errors = "";
                    foreach (var eml in emails.Messages)
                    {
                        var msgPara = new ForwardMessageRequest();
                        msgPara.Id = eml.Id;
                        msgPara.NewBCC = Para.NewBCC;
                        msgPara.NewCC = Para.NewCC;
                        msgPara.NewTo = Para.NewTo;
                        var resp = await ForwardMessage(msgPara);
                        if (resp != null && resp.Message != "Success.")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success.";
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    response.Message = ex.Message;
                    return response;
                }
            }
            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ForwardEmail]response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("reply"), HttpPost("~/skill/{controller}/reply")]
        public async Task<ServerResponse> ReplyEmail(ReplyEmailRequest Para)
        {
            //according to the rfc822 standard:
            /* https://datatracker.ietf.org/doc/html/rfc2822#section-3.6.4
               3.6.4. Identification fields
               "...reply messages SHOULD have "In-Reply-To:" and
               "References:" fields as appropriate, as described below...
               The "References:" and "In-Reply-To:" field each contain one or more
               unique message identifiers, optionally separated by CFWS."
             */
            //EXAMPLE PROMPT: search for an email containing XYZ in the subject and send a reply to it with new body of "new body test"
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyEmail]");
            var response = new GetEmailsResponse();
            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            Response.StatusCode = 200;
            QueryEmailsResponse emails = await QueryEmails(Para);
            if (emails != null && emails.Messages != null && emails.Messages.Count > 0)
            {
                try
                {
                    string errors = "";
                    foreach (var eml in emails.Messages)
                    {
                        var msgPara = new ReplyMessageRequest();
                        msgPara.Id = eml.Id;
                        msgPara.NewCC = Para.NewCC;
                        msgPara.NewBCC = Para.NewBCC;
                        msgPara.NewSubject = Para.NewSubject;
                        msgPara.NewBody = Para.NewBody;
                        var resp = await ReplyMessage(msgPara);
                        if (resp != null && resp.Message != "Success.")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success.";
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    response.Message = ex.Message;
                    return response;
                }
            }
            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyEmail]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Label Management
        
        [HttpPost("query_and_add_label"),HttpPost("~/skill/{controller}/query_and_add_label")]
        public async Task<ServerResponse> QueryEmailAndAddLabel(QueryEmailAndAddLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][QueryEmailAndAddLabel]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                var sp = Para.GetSearchFilters();
                var qresp = await QueryEmails(sp);
                if(qresp != null && qresp.Messages != null)
                {
                    string err = "";
                    foreach(var msg in qresp.Messages)
                    {
                        var para2 = new AddLabelRequest();
                        para2.Id = msg.Id;
                        para2.Label = Para.AddLabel;
                        var resp = await AddLabelToMessage(para2);
                        if (resp != null)
                        {
                            if (Has(resp.Message))
                            {
                                err = AppendString(err, resp.Message);
                            }
                        }
                    }
                    if(!Has(err))
                    {
                        response.Message = "Success.";
                    }
                    else
                    {
                        response.Message = err;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][QueryEmailAndAddLabel]response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("query_and_remove_label"),HttpPost("~/skill/{controller}/query_and_remove_label")]
        public async Task<ServerResponse> QueryEmailAndRemoveLabel(QueryEmailAndRemoveLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][QueryEmailAndRemoveLabel]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                var sp = Para.GetSearchFilters();
                var qresp = await QueryEmails(sp);
                if(qresp != null && qresp.Messages != null)
                {
                    string err = "";
                    foreach(var msg in qresp.Messages)
                    {
                        var para2 = new RemoveLabelRequest();
                        para2.Id = msg.Id;
                        para2.Label = Para.RemoveLabel;
                        var resp = await RemoveLabelFromMessage(para2);
                        if (resp != null)
                        {
                            if (Has(resp.Message))
                            {
                                err = AppendString(err, resp.Message);
                            }
                        }
                    }
                    if(!Has(err))
                    {
                        response.Message = "Success.";
                    }
                    else
                    {
                        response.Message = err;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][QueryEmailAndRemoveLabel]response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("add_label"),HttpPost("~/skill/{controller}/add_label")]
        public async Task<ServerResponse> AddLabel(AddLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][AddLabel]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                if (IsJsonObject(Para.Id))
                {
                    var msgs = GetIdsFromMessageObj(Para.Id);
                    string errors = "";
                    foreach (var msg in msgs)
                    {
                        Para.Id = msg.Id;
                        var resp = await AddLabelToMessage(Para);
                        if (resp != null && resp.Message != "")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success.";
                    }
                }
                else
                {
                    var resp = await AddLabelToMessage(Para);
                    if (resp != null && resp.Message == "")
                    {
                        response.Message = "Success.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][AddLabel]response:" + JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("remove_label"),HttpPost("~/skill/{controller}/remove_label")]
        public async Task<ServerResponse> RemoveLabel(RemoveLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][RemoveLabel]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized."; 
                return response;
            }
            try
            {
                if (IsJsonObject(Para.Id))
                {
                    var msgs = GetIdsFromMessageObj(Para.Id);
                    string errors = "";
                    foreach (var msg in msgs)
                    {
                        Para.Id = msg.Id;
                        var resp = await RemoveLabelFromMessage(Para);
                        if (resp != null && resp.Message != "")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success.";
                    }
                }
                else
                {
                    var resp = await RemoveLabelFromMessage(Para);
                    if (resp != null && resp.Message == "")
                    {
                        response.Message = "Success.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            if (!Has(response.Message))
            {
                response.Message = "Success.";
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][RemoveLabel]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion
    }
}
