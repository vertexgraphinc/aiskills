using GMail.Contracts;
using GMail.GmailClient;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Google.Apis.Requests.BatchRequest;
using static System.Net.Mime.MediaTypeNames;

namespace GMail.Helpers
{
    public class MessagesHelpers : ExpandoHelpers
    {
        int _defaultMaxResults = 5;
        public async Task<ExpandoObject> GetMessage(string Id)
        {
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/get
            return await Get<ExpandoObject>($"messages/{Id}?format=full");
        }
        public async Task<List<GMailMessage>> ListMessages(SearchFilters Para, bool ReturnfullMessages = true)
        {
            var RetMsgs = new List<GMailMessage>();

            if (!HasAtLeastOneProp(new string[] { Para.From, Para.Label, Para.Subject, Para.Body, Para.Status, Para.BeginTime, Para.EndTime })) {
                throw new Exception("At least one search parameter is necessary.");
            }

            string from = GetOptionalQStringParam("from", Para.From);
            string label = GetOptionalQStringParam("label", Para.Label);
            string subject = GetOptionalQStringParam("subject", Para.Subject);
            string body = GetBodyQuery(Para.Body);
            string status = GetOptionalQStringParam("is", Para.Status);
            string dateQuery = GetSentQuery(Para.BeginTime, Para.EndTime);
            string searchParams = AssembleSearchParams(from, label, subject, body, status, dateQuery);

            //reference: https://support.google.com/mail/answer/7190
            var expMessages = await Get<ExpandoObject>($"messages?maxResults={_defaultMaxResults}&{searchParams}");
            if (!Has(expMessages))
                return RetMsgs;

            if (expMessages is IDictionary<string, object>)
            {
                try
                {
                    var dict = (IDictionary<string, object>)expMessages;
                    if (dict.ContainsKey("messages"))
                    {
                        object allMessages = dict["messages"];
                        if (allMessages is List<object>)
                        {
                            foreach (var obj in (List<object>)allMessages)
                            {
                                var props = GetDict(obj);
                                if (props.ContainsKey("id"))
                                {
                                    var message = new GMailMessage();
                                    message.Id = GetStringVal(props["id"]);
                                    if (ReturnfullMessages)
                                    {
                                        //retrive the full message based on the id property
                                        var dmsg = await GetMessage(message.Id);
                                        if (dmsg != null)
                                        {
                                            var subProps = GetDict(dmsg);
                                            if (subProps.ContainsKey("snippet"))
                                            {
                                                message.Snippet = GetStringVal(subProps["snippet"]);
                                            }
                                            if (subProps.ContainsKey("payload"))
                                            {
                                                var gmailClient = new GMailClientMessage(subProps["payload"]);
                                                message.From = gmailClient.From;
                                                message.To = gmailClient.To;
                                                message.Subject = gmailClient.Subject;
                                                message.Received = gmailClient.Received;
                                                message.Body = gmailClient.Body;
                                                RetMsgs.Add(message);
                                            }
                                        }
                                        else
                                        {
                                            RetMsgs.Add(message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return RetMsgs;
        }
        public async Task<List<GMailThread>> ListThreads(SearchFilters Para)
        {
            var RetThreads = new List<GMailThread>();                                                         

            if (!HasAtLeastOneProp(new string[] { Para.From, Para.Label, Para.Subject, Para.Body, Para.Status, Para.BeginTime, Para.EndTime }))
            {
                throw new Exception("At least one search parameter is necessary.");
            }

            string from = GetOptionalQStringParam("from", Para.From);
            string label = GetOptionalQStringParam("label", Para.Label);
            string subject = GetOptionalQStringParam("subject", Para.Subject);
            string body = GetBodyQuery(Para.Body);
            string status = GetOptionalQStringParam("is", Para.Status);
            string dateQuery = GetSentQuery(Para.BeginTime, Para.EndTime);
            string searchParams = AssembleSearchParams(from, label, subject, body, status, dateQuery);

            //reference: https://support.google.com/mail/answer/7190
            var expMessages = await Get<ExpandoObject>($"threads?maxResults={_defaultMaxResults}&{searchParams}");
            if (!Has(expMessages))
                return RetThreads;

            if (expMessages is IDictionary<string, object>)
            {
                try
                {
                    var dict = (IDictionary<string, object>)expMessages;
                    if (dict.ContainsKey("threads"))
                    {
                        object allMessages = dict["threads"];
                        if (allMessages is List<object>)
                        {
                            foreach (var obj in (List<object>)allMessages)
                            {
                                var props = GetDict(obj);
                                if (props.ContainsKey("id"))
                                {
                                    var message = new GMailThread();
                                    message.Id = GetStringVal(props["id"]);
                                    message.ThreadId = GetStringVal(props["id"]);
                                    //retrive the full message based on the id property
                                    var dmsg = await GetMessage(message.Id);
                                    if (dmsg != null)
                                    {
                                        var subProps = GetDict(dmsg);
                                        if (subProps.ContainsKey("snippet"))
                                        {
                                            message.Snippet = SanitizeString(GetStringVal(subProps["snippet"]));
                                        }
                                        if (subProps.ContainsKey("payload"))
                                        {
                                            var gmailClient = new GMailClientMessage(subProps["payload"]);
                                            message.From = gmailClient.From;
                                            message.To = gmailClient.To;
                                            message.Subject = gmailClient.Subject;
                                            message.Received = gmailClient.Received;
                                            message.Body = gmailClient.Body;
                                            RetThreads.Add(message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(SanitizeString(ex.Message));
                }
            }
            return RetThreads;
        }
        #region Send Messages
        public async Task<ServerResponse> SendMessage(SendEmailRequest Para)
        {
            var response = new ServerResponse();
            if (!Has(Para.To) || !Has(Para.Body))
            {
                response.MessageFromServer = "Missing parameter. In order to send an email, you need at least a To, a Subject, and a Body.";
            }
            if (!Has(Para.Subject))
            {
                Para.Subject = TrimIfTooLong(Para.Body, 50);
            }
            var message = new StringBuilder("", 1024);          
            message.AppendLine($"To: {Para.To}");   
            if (Has(Para.ThreadId))
            {
                message.AppendLine($"ThreadId: {Para.ThreadId}");
            }
            if (Has(Para.CC))
            {
                message.AppendLine($"CC: {Para.CC}");
            }
            if (Has(Para.BCC))
            {
                message.AppendLine($"BCC: {Para.BCC}");
            }
            message.AppendLine();
            message.AppendLine(Para.Body);
            try
            {
                var RawMessage = EncodeBodyText(message.ToString());
                using (HttpClient client = new HttpClient())
                {
                    string Token = GetSessionToken();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");

                    if (Has(Para.Subject))
                    {
                        client.DefaultRequestHeaders.Add("Subject", Para.Subject);
                    }
                    if (Has(Para.InReplyToId))
                    {
                        client.DefaultRequestHeaders.Add("In-Reply-To", Para.InReplyToId);
                        client.DefaultRequestHeaders.Add("References", Para.ReferencesId);
                    }
                    var content = new StringContent($"{{\"raw\":\"{RawMessage}\"}}", Encoding.UTF8, "application/json");
                    var resp = await client.PostAsync("https://gmail.googleapis.com/gmail/v1/users/me/messages/send", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        response.MessageFromServer = "";
                    }
                    else 
                    { 
                        var errorMessage = await resp.Content.ReadAsStringAsync();
                        response.MessageFromServer = $"Failed to send email. Status Code: {resp.StatusCode}, Message: {errorMessage}";
                    }
                }
            }
            catch (Exception ex)
            {
                response.MessageFromServer = ex.Message;
            }
            return response;
        }
        public async Task<ServerResponse> ReplyToEmail(ReplyToEmailRequest Para)
        {            
            var response = new ServerResponse();
            string inReplyToId = Para.Id;
            string subject = GetOptionalQStringParam("new_subject", Para.NewSubject);
            string body = GetBodyQuery(Para.NewBody);

            if (!Has(inReplyToId))
            {
                throw new Exception("Message Id is not specified.");
            }           
            if (!IsAlphaNumeric(inReplyToId))
            {
                throw new Exception("Invalid message Id: " + SanitizeString(inReplyToId));
            }
            if (!Has(Para.NewBody))
            {
                throw new Exception("Message body is not specified.");
            }
            var origMessage = new GMailMessage();
            origMessage.Id = inReplyToId;
            //retrive the full message based on the id property
            var dmsg = await GetMessage(inReplyToId);
            if (!Has(dmsg))
            {
                throw new Exception("Invalid message Id.");
            }
            else
            {
                var subProps = GetDict(dmsg);
                if (subProps.ContainsKey("payload"))
                {
                    var gmailClient = new GMailClientMessage(subProps["payload"]);
                    origMessage.From = gmailClient.From;
                    origMessage.To = gmailClient.To;
                    origMessage.Subject = gmailClient.Subject;
                    origMessage.Received = gmailClient.Received;
                    origMessage.Body = gmailClient.Body;
               
                    var newEmail = new SendEmailRequest();
                    newEmail.To = origMessage.From;
                    newEmail.InReplyToId = origMessage.Id;
                    string oldSubject = origMessage.Subject;
                    if (!Has(Para.NewSubject))
                    {
                        newEmail.Subject = oldSubject;
                    }
                    else
                    {
                        newEmail.Subject = GetStringVal(Para.NewSubject);
                    }
                    newEmail.Body = Para.NewBody;
                    response = await SendMessage(newEmail);
                }
            }
            return response;
        }
        public async Task<ServerResponse> FowardEmail(ForwardEmailRequest Para)
        {
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.MessageFromServer = "Message Id is not specified.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + SanitizeString(Para.Id));
            }
            if (!Has(Para.NewTo))
            {
                response.MessageFromServer = "To is not specified.";
            }
            var origMessage = new GMailMessage();
            origMessage.Id = Para.Id;
            //retrive the full message based on the id property
            var dmsg = await GetMessage(Para.Id);
            if (!Has(dmsg))
            {
                response.MessageFromServer = "Invalid message Id.";
            }
            else
            {
                var subProps = GetDict(dmsg);
                if (subProps.ContainsKey("payload"))
                {
                    var gmailClient = new GMailClientMessage(subProps["payload"]);
                    origMessage.From = gmailClient.From;
                    origMessage.To = gmailClient.To;
                    origMessage.Subject = gmailClient.Subject;
                    origMessage.Body = gmailClient.Body;

                    var newEmail = new SendEmailRequest();
                    newEmail.To = origMessage.From;
                    if (Has(Para.NewCC))
                    {
                        newEmail.CC = Para.NewCC;
                    }
                    if (Has(Para.NewBCC))
                    {
                        newEmail.BCC = Para.NewBCC;
                    }
                    newEmail.ReferencesId = origMessage.Id;
                    newEmail.Subject = origMessage.Subject;
                    newEmail.Body = origMessage.Body;
                    response = await SendMessage(newEmail);
                }
            }
            return response;
        }
        #endregion

        #region Label Management
        protected async Task<string> CreateLabel(string labelName)
        {
            //https://developers.google.com/gmail/api/reference/rest/v1/users.labels/create
            var requestBodyObj = new Dictionary<string, string>()
            {
                { "labelListVisibility", "labelShow" },
                { "messageListVisibility", "show" },
                { "name", labelName}
            };
            string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
            var result = await Post<ExpandoObject>("labels", jsonRequestBody);
            if (result != null)
            {
                return "";
            }
            if (result is IDictionary<string, object>)
            {
                var dict = (IDictionary<string, object>)result;
                if (dict.ContainsKey("id"))
                {
                    return GetStringVal(dict["id"]);
                }
            }
            return "";
        }
        public async Task<ServerResponse> AddLabelToMessage(AddLabelToMessageRequest Para)
        {
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.MessageFromServer = "A message Id is required.";
            }
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/modify
            string labelId = await FindLabelId(Para.AddLabel);
            if (!Has(labelId))
            {
                //label doesn't exist, create it first
                labelId = await CreateLabel(Sanitize(Para.AddLabel));
            }
            if (Has(labelId))
            {
                var requestBodyObj = new Dictionary<string, string[]>()
                {
                    { "addLabelIds", new string[] { labelId } }
                };
                string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
                var result = await Post<ExpandoObject>($"messages/{Para.Id}/modify", jsonRequestBody);
                if (!Has(result))
                {
                    response.MessageFromServer = "Unknown error 1. Could not add label.";
                }
                if (result is IDictionary<string, object>)
                {
                    var dict = (IDictionary<string, object>)result;
                    if (dict.ContainsKey("id"))
                    {
                        if (!Has(GetStringVal(dict["id"])))
                        {
                            response.MessageFromServer = "Unknown error 2. Could not add label.";
                        }
                        else
                        {
                            response.MessageFromServer = "";
                        }
                    }
                }
            }
            return response;
        }
        public async Task<ServerResponse> RemoveLabelFromMessage(RemoveLabelFromMessageRequest Para)
        {
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.MessageFromServer = "A message Id is required.";
            }
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/modify
            string labelId = await FindLabelId(Para.RemoveLabel);
            if (!Has(labelId))
            {
                response.MessageFromServer = "The label doesn't exist.";
            }
            if (Has(labelId))
            {
                var requestBodyObj = new Dictionary<string, string[]>()
                {
                    { "removeLabelIds", new string[] { labelId } }
                };
                string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
                var result = await Post<ExpandoObject>($"messages/{Para.Id}/modify", jsonRequestBody);
                if (result == null)
                {
                    response.MessageFromServer = "Unknown error 1. Could not remove the label.";
                }
                if (result is IDictionary<string, object>)
                {
                    var dict = (IDictionary<string, object>)result;
                    if (dict.ContainsKey("id"))
                    {
                        if (!Has(GetStringVal(dict["id"])))
                        {
                            response.MessageFromServer = "Unknown error 2. Could not remove the label.";
                        }
                        else
                        {                         
                            response.MessageFromServer = "";
                        }
                    }
                }
            }
            return response;
        }
        protected async Task<string> FindLabelId(string labelName)
        {
            if (!Has(labelName))
            {
                return "";
            }
            //https://developers.google.com/gmail/api/reference/rest/v1/users.labels/list?apix_params=%7B%22userId%22%3A%22valterk%40gladinet.com%22%7D
            var labels = await Get<ExpandoObject>("labels");
            if (!Has(labels))
            {
                return "";
            }
            if (labels is IDictionary<string, object>)
            {
                var dict = (IDictionary<string, object>)labels;
                if (dict.ContainsKey("labels"))
                {
                    object allLabels = dict["labels"];
                    if (allLabels is List<object>)
                    {
                        foreach (var obj in (List<object>)allLabels)
                        {
                            var id = "";
                            var name = "";
                            var props = GetDict(obj);
                            if (props.ContainsKey("id"))
                            {
                                id = GetStringVal(props["id"]);
                            }
                            if (props.ContainsKey("name"))
                            {
                                name = GetStringVal(props["name"]);
                            }
                            if (labelName.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                            {
                                return id;
                            }
                        }
                    }
                }
            }
            return "";
        }
        #endregion
    }
}