using GMail.Contracts;
using GMail.GMailClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GMail.Helpers
{
    public class MessagesHelpers : OAuthSession
    {
        int _defaultMaxResults = 5;

        #region Fetching Data
        public async Task<List<GMailMessage>> ListMessages(SearchFilters Para)
        {
            var RetMsgs = new List<GMailMessage>();
            
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

            System.Diagnostics.Debug.WriteLine("[vertex]:searchParams:" + searchParams);

            //reference: https://support.google.com/mail/answer/7190
            var results = await Get<GMailClientMessages>($"messages?maxResults={_defaultMaxResults}&{searchParams}");
            if (results == null || results.Messages == null)
                return RetMsgs;

            System.Diagnostics.Debug.WriteLine("[vertex]:searchParams:" + searchParams);
            foreach (var msg in results.Messages)
            {
                string msgId = msg.Id;
                string msgFrom = "";
                string msgTo = "";
                string msgSubject = "";
                string msgSnippet = "";
                var msgBody = new StringBuilder("", 2048);
                string msgDate = "";

                var fullMsg = await GetMessage(msgId);
                if (!Has(fullMsg))
                    continue;

                if (!Has(fullMsg.Payload))
                    continue;

                if (!Has(fullMsg.Payload.Headers))
                    continue;

                foreach (var header in fullMsg.Payload.Headers)
                {
                    switch (header.Name.ToLower())
                    {
                        case "from":
                            msgFrom = header.Value;
                            break;
                        case "to":
                            msgTo = header.Value;
                            break;
                        case "subject":
                            msgSubject = header.Value;
                            break;
                        case "date":
                            msgDate = header.Value;
                            break;
                    }
                }
                msgSnippet = StripHtmlTags(fullMsg.Snippet);
                if (Has(fullMsg.Payload.Parts))
                {
                    //for multi-part messages, extract the body from all the parts
                    foreach (var part in fullMsg.Payload.Parts)
                    {
                        if (Has(part.Parts))
                        {
                            foreach (var subPart in part.Parts)
                            {
                                if (Has(subPart.Body) && Has(subPart.Body.Data))
                                {
                                    msgBody.Append("\n-----------------------------------\n");
                                    msgBody.Append(DecodeBase64String(subPart.Body.Data));
                                }
                            }
                        }
                        if (Has(part.Body) && Has(part.Body.Data))
                        {
                            msgBody.Append("\n-----------------------------------\n");
                            msgBody.Append(DecodeBase64String(part.Body.Data));
                        }
                    }
                }
                //if there is a body on the root
                if (Has(fullMsg.Payload.Body) && Has(fullMsg.Payload.Body.Data))
                {
                    msgBody.Append("\n-----------------------------------\n");
                    msgBody.Append(DecodeBase64String(fullMsg.Payload.Body.Data));
                }

                msgSubject = Sanitize(StripHtmlTags(msgSubject));
                msgSnippet = Sanitize(StripHtmlTags(msgSnippet));
                string finalBody = Sanitize(StripHtmlTags(msgBody.ToString()));
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgId:" + msgId);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgTo:" + msgTo);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgFrom:" + msgFrom);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgSubject:" + msgSubject);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgSnippet:" + msgSnippet);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:finalBody:" + finalBody);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:msgDate:" + msgDate);
                System.Diagnostics.Debug.WriteLine("[vertex][ListMessages]:==================================");

                RetMsgs.Add(new GMailMessage
                {
                    Id = msgId,
                    ThreadId = msg.ThreadId,
                    To = msgTo,
                    From = msgFrom,
                    Subject = msgSubject,
                    Snippet = msgSnippet,
                    Body = finalBody,
                    Received = msgDate
                });
            }
            return RetMsgs;
        }
        public async Task<List<GMailMessage>> ListThreads(SearchFilters Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex]:ListThreads");
            var messages = new List<GMailMessage>();

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

            System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:searchParams:" + searchParams);

            //reference: https://support.google.com/mail/answer/7190
            var results = await Get<GMailClientThreads>($"threads?maxResults={_defaultMaxResults}&{searchParams}");
            if (results == null || results.Threads == null)
                return messages;

            System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:results.Threads.Count:" + results.Threads.Count.ToString());
            foreach (var thrd in results.Threads)
            {
                //messages come back empty from Gmail API. Need to fetch the thread messages with a second call
                var results2 = await Get<GMailClientMessages>($"threads/{thrd.Id}?format=full");
                if (results2.Messages == null)
                {
                    continue;
                }
                System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:results2.Messages.Count:" + results2.Messages.Count.ToString());
                //each thread has a messages list
                foreach (var msg in results2.Messages)
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msg.Id:" + msg.Id);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msg.ThreadId:" + msg.ThreadId);
                    string msgId = msg.Id;
                    string msgFrom = "";
                    string msgTo = "";
                    string msgSubject = "";
                    string msgSnippet = "";
                    var msgBody = new StringBuilder("", 2048);
                    string msgDate = "";

                    var fullMsg = await GetMessage(msgId);
                    if (!Has(fullMsg))
                        continue;

                    if (!Has(fullMsg.Payload))
                        continue;

                    if (!Has(fullMsg.Payload.Headers))
                        continue;

                    foreach (var header in fullMsg.Payload.Headers)
                    {
                        switch (header.Name.ToLower())
                        {
                            case "from":
                                msgFrom = header.Value;
                                break;
                            case "to":
                                msgTo = header.Value;
                                break;
                            case "subject":
                                msgSubject = header.Value;
                                break;
                            case "date":
                                msgDate = header.Value;
                                break;
                        }
                    }
                    msgSnippet = StripHtmlTags(fullMsg.Snippet);
                    if (Has(fullMsg.Payload.Parts))
                    {
                        //for multi-part messages, extract the body from all the parts
                        foreach (var part in fullMsg.Payload.Parts)
                        {
                            if (Has(part.Parts))
                            {
                                foreach (var subPart in part.Parts)
                                {
                                    if (Has(subPart.Body) && Has(subPart.Body.Data))
                                    {
                                        msgBody.Append("\n-----------------------------------\n");
                                        msgBody.Append(DecodeBase64String(subPart.Body.Data));
                                    }
                                }
                            }
                            if (Has(part.Body) && Has(part.Body.Data))
                            {
                                msgBody.Append("\n-----------------------------------\n");
                                msgBody.Append(DecodeBase64String(part.Body.Data));
                            }
                        }
                    }
                    //if there is a body on the root
                    if (Has(fullMsg.Payload.Body) && Has(fullMsg.Payload.Body.Data))
                    {
                        msgBody.Append("\n-----------------------------------\n");
                        msgBody.Append(DecodeBase64String(fullMsg.Payload.Body.Data));
                    }

                    msgSubject = Sanitize(StripHtmlTags(msgSubject));
                    msgSnippet = Sanitize(StripHtmlTags(msgSnippet));
                    string finalBody = Sanitize(StripHtmlTags(msgBody.ToString()));
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgId:" + msgId);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:thrd.Id:" + thrd.Id);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgTo:" + msgTo);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgFrom:" + msgFrom);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgSubject:" + msgSubject);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgSnippet:" + msgSnippet);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:finalBody:" + finalBody);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:msgDate:" + msgDate);
                    System.Diagnostics.Debug.WriteLine("[vertex][ListThreads]:==================================");

                    messages.Add(new GMailMessage
                    {
                        Id = msgId,
                        ThreadId = thrd.Id,
                        To = msgTo,
                        From = msgFrom,
                        Subject = msgSubject,
                        Snippet = msgSnippet,
                        Body = finalBody,
                        Received = msgDate
                    });
                }
            }
            return messages;
        }
        protected List<GMailClientMessage> GetIdsFromMessageObj(string jsonStr)
        {
            System.Diagnostics.Debug.WriteLine("[vertex]:GetIdFromMessageObj");
            var msgs = new List<GMailClientMessage>();
            //if the system passes the entire message object instead of just the message id
            //extract the id
            try
            {
                var result = JsonConvert.DeserializeObject<GMailClientMessages>(jsonStr);
                foreach (var msg in result.Messages)
                {
                    System.Diagnostics.Debug.WriteLine("[vertex]:GetIdFromMessageObj:add:" + msg.Id);
                    msgs.Add(msg);
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[vertex]:GetIdFromMessageObj:ex:" + ex.Message);
            }
            return msgs;
        }
        public async Task<GMailMessage> GetMessage(GetEmailRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][GetEmailRequest]");
            if (!Has(Para.Id))
            {
                throw new Exception("Message Id is not specified.");
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            var origMessage = new GMailMessage();
            origMessage.Id = Para.Id;

            //retrieve the full message based on the id property
            var fullMsg = await GetMessage(Para.Id);
            if (!Has(fullMsg))
            {
                throw new Exception("Invalid message Id.");
            }
            else
            {
                string origFrom = "", origCC = "", origSubject = "", origBody = "", origDate = "";
                if (Has(fullMsg.Payload))
                {
                    if (Has(fullMsg.Payload.Headers))
                    {
                        foreach (var header in fullMsg.Payload.Headers)
                        {
                            switch (header.Name.ToLower())
                            {
                                case "from":
                                    origFrom = header.Value;
                                    break;
                                case "cc":
                                    origCC = header.Value;
                                    break;
                                case "subject":
                                    origSubject = header.Value;
                                    break;
                                case "date":
                                    origDate = header.Value;
                                    break;
                            }
                        }
                    }
                    var msgBody = new StringBuilder("", 256);
                    if (Has(fullMsg.Payload.Parts))
                    {
                        //for multi-part messages, extract the body from all the parts
                        foreach (var part in fullMsg.Payload.Parts)
                        {
                            if (Has(part.Parts))
                            {
                                foreach (var subPart in part.Parts)
                                {
                                    if (Has(subPart.Body) && Has(subPart.Body.Data))
                                    {
                                        msgBody.Append("\n-----------------------------------\n");
                                        msgBody.Append(DecodeBase64String(subPart.Body.Data));
                                    }
                                }
                            }
                            if (Has(part.Body) && Has(part.Body.Data))
                            {
                                msgBody.Append("\n-----------------------------------\n");
                                msgBody.Append(DecodeBase64String(part.Body.Data));
                            }
                        }
                    }
                    //if there is a body on the root
                    if (Has(fullMsg.Payload.Body) && Has(fullMsg.Payload.Body.Data))
                    {
                        msgBody.Append("\n-----------------------------------\n");
                        msgBody.Append(DecodeBase64String(fullMsg.Payload.Body.Data));
                    }
                    origBody = Sanitize(StripHtmlTags(msgBody.ToString()));
                }
                origMessage.ThreadId = fullMsg.ThreadId;
                origMessage.From = origFrom;
                origMessage.Subject = origSubject;
                origMessage.Snippet = fullMsg.Snippet;
                origMessage.Body = origBody;
                origMessage.Received = origDate;
            }
            return origMessage;
        }
        #endregion

        #region Sending Emails
        protected string MakeRFC822Message(string inReplyToId, string to, string cc, string bcc, string subject, string body)
        {
            //returns the base64 encoded RFC822 compliant string for the raw post body
            var msg = new StringBuilder();
            // headers
            msg.AppendLine("From: me");
            
            if (Has(to))
            {
                msg.AppendLine("To: " + to);
            }
            if (Has(cc))
            {
                msg.AppendLine("Cc: " + cc);
            }
            if (Has(bcc))
            {
                msg.AppendLine("Bcc: " + bcc);
            }
            if (Has(subject))
            {
                msg.AppendLine("Subject: " + subject);
            }
            msg.AppendLine("Date: " + DateTime.Now.ToString("R")); // RFC822 date format

            if (Has(inReplyToId))
            {
                msg.AppendLine("In-Reply-To: " + inReplyToId);
                msg.AppendLine("References: " + inReplyToId);
            }

            // MIME-Version and Content-Type headers
            msg.AppendLine("MIME-Version: 1.0");
            msg.AppendLine("Content-Type: text/html; charset=utf-8");
            // blank line to separate headers from the body
            msg.AppendLine();
            msg.AppendLine(body);
            System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:msg:" + msg.ToString());
            var RawMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(msg.ToString()));
            System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:encoded message:" + RawMessage);
            return RawMessage;
        }
        public async Task<ServerResponse> SendMessage(SendEmailRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]");
            var response = new ServerResponse();
            if (!Has(Para.To) || !Has(Para.Body))
            {
                response.Message = "Missing parameter. In order to send an email, you need at least a To, a Subject, and a Body.";
            }
            if (!Has(Para.Subject))
            {
                Para.Subject = TrimIfTooLong(Para.Body, 50);
            }
            var RawMessage = MakeRFC822Message(Para.InReplyToId, Para.To, Para.CC, Para.BCC, Para.Subject, Para.Body);
            //---------------------------------------------------------
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Token = GetSessionToken();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
                    client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");

                    System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:Token:" + Token);
                    if (Has(Para.InReplyToId))
                    {
                        System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:InReplyToId:" + Para.InReplyToId);
                    }
                    var content = new StringContent($"{{\"raw\":\"{RawMessage}\"}}", Encoding.UTF8, "application/json");

                    System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:RawMessage:" + RawMessage);

                    var resp = await client.PostAsync("https://gmail.googleapis.com/gmail/v1/users/me/messages/send", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:Success");
                        response.Message = "";
                    }
                    else
                    {
                        var errorMessage = await resp.Content.ReadAsStringAsync();
                        response.Message = $"Failed to send email. Status Code: {resp.StatusCode}, Message: {errorMessage}";
                        System.Diagnostics.Debug.WriteLine("[vertex][SendMessage]:Message:" + Sanitize(StripHtmlTags(errorMessage)));
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServerResponse> ForwardMessage(ForwardEmailRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][ForwardMessage]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "Message Id is not specified.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            System.Diagnostics.Debug.WriteLine("[vertex][ForwardMessage]Para.Id:" + Para.Id);
            if (!Has(Para.NewTo))
            {
                response.Message = "To is not specified.";
            }
            System.Diagnostics.Debug.WriteLine("[vertex][ForwardMessage]Para.NewTo:" + Para.NewTo);
            var origMessage = new GMailMessage();
            origMessage.Id = Para.Id;

            //retrieve the full message based on the id property
            var fullMsg = await GetMessage(Para.Id);
            if (!Has(fullMsg))
            {
                response.Message = "Invalid message Id.";
            }
            else
            {
                string origFrom = "", origCC = "", origSubject = "", origBody = "";
                if (Has(fullMsg.Payload))
                {
                    if (Has(fullMsg.Payload.Headers))
                    {

                        foreach (var header in fullMsg.Payload.Headers)
                        {
                            switch (header.Name.ToLower())
                            {
                                case "from":
                                    origFrom = header.Value;
                                    break;
                                case "cc":
                                    origCC = header.Value;
                                    break;
                                case "subject":
                                    origSubject = header.Value;
                                    break;
                            }
                        }
                    }
                    var msgBody = new StringBuilder("", 256);
                    if (Has(fullMsg.Payload.Parts))
                    {
                        //for multi-part messages, extract the body from all the parts
                        foreach (var part in fullMsg.Payload.Parts)
                        {
                            if (Has(part.Parts))
                            {
                                foreach (var subPart in part.Parts)
                                {
                                    if (Has(subPart.Body) && Has(subPart.Body.Data))
                                    {
                                        msgBody.Append("\n-----------------------------------\n");
                                        msgBody.Append(DecodeBase64String(subPart.Body.Data));
                                    }
                                }
                            }
                            if (Has(part.Body) && Has(part.Body.Data))
                            {
                                msgBody.Append("\n-----------------------------------\n");
                                msgBody.Append(DecodeBase64String(part.Body.Data));
                            }
                        }
                    }
                    //if there is a body on the root
                    if (Has(fullMsg.Payload.Body) && Has(fullMsg.Payload.Body.Data))
                    {
                        msgBody.Append("\n-----------------------------------\n");
                        msgBody.Append(DecodeBase64String(fullMsg.Payload.Body.Data));
                    }
                    origBody = Sanitize(StripHtmlTags(msgBody.ToString()));
                }

                System.Diagnostics.Debug.WriteLine("[vertex][ForwardMessage]origSubject:" + origSubject);
                System.Diagnostics.Debug.WriteLine("[vertex][ForwardMessage]origBody:" + origBody);
                var newEmail = new SendEmailRequest();
                newEmail.To = Para.NewTo;
                newEmail.CC = Para.NewCC;
                newEmail.BCC = Para.NewBCC;
                newEmail.Subject = "FW: " + origSubject;
                newEmail.Body = origBody;
                response = await SendMessage(newEmail);
            }
            return response;
        }
        protected bool IsJsonObject(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return false;
            try
            {
                // Attempt to parse the string as JSON
                JsonDocument.Parse(input);
                System.Diagnostics.Debug.WriteLine("[vertex]:IsJsonObject:true:" + input);
                return true;
            }
            catch (System.Text.Json.JsonException)
            {
                // Parsing failed, not a valid JSON
                System.Diagnostics.Debug.WriteLine("[vertex]:IsJsonObject:false:" + input);
                return false;
            }
        }
        public async Task<ServerResponse> ReplyMessage(ReplyEmailRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][ReplyMessage]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "Message Id is not specified.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            if (!Has(Para.NewBody))
            {
                response.Message = "New Body is not specified.";
            }
            var origMessage = new GMailMessage();
            origMessage.Id = Para.Id;

            //retrieve the full message based on the id property
            var fullMsg = await GetMessage(Para.Id);
            if (!Has(fullMsg))
            {
                response.Message = "Invalid message Id.";
            }
            else
            {
                string origFrom = "", origCC = "", origSubject = "", origBody = "", origDate = "";
                if (Has(fullMsg.Payload))
                {
                    if (Has(fullMsg.Payload.Headers))
                    {

                        foreach (var header in fullMsg.Payload.Headers)
                        {
                            switch (header.Name.ToLower())
                            {
                                case "from":
                                    origFrom = header.Value;
                                    break;
                                case "cc":
                                    origCC = header.Value;
                                    break;
                                case "subject":
                                    origSubject = header.Value;
                                    break;
                                case "date":
                                    origDate = header.Value;
                                    break;
                            }
                        }
                    }
                    var msgBody = new StringBuilder("", 256);
                    if (Has(fullMsg.Payload.Parts))
                    {
                        //for multi-part messages, extract the body from all the parts
                        foreach (var part in fullMsg.Payload.Parts)
                        {
                            if (Has(part.Parts))
                            {
                                foreach (var subPart in part.Parts)
                                {
                                    if (Has(subPart.Body) && Has(subPart.Body.Data))
                                    {
                                        msgBody.Append("\n-----------------------------------\n");
                                        msgBody.Append(DecodeBase64String(subPart.Body.Data));
                                    }
                                }
                            }
                            if (Has(part.Body) && Has(part.Body.Data))
                            {
                                msgBody.Append("\n-----------------------------------\n");
                                msgBody.Append(DecodeBase64String(part.Body.Data));
                            }
                        }
                    }
                    //if there is a body on the root
                    if (Has(fullMsg.Payload.Body) && Has(fullMsg.Payload.Body.Data))
                    {
                        msgBody.Append("\n-----------------------------------\n");
                        msgBody.Append(DecodeBase64String(fullMsg.Payload.Body.Data));
                    }
                    origBody = Sanitize(StripHtmlTags(msgBody.ToString()));
                }
                string newCC = origCC;
                if (Has(Para.NewCC))
                {
                    newCC = Para.NewCC;
                }
                string newSubject = "Re: ";
                if (Has(origSubject))
                {
                    newSubject = "Re: " + origSubject;
                }
                if (Has(Para.NewSubject))
                {
                    newSubject = "Re: " + Para.NewSubject;
                }
                string replyInfo = "<br /><br />";
                if (Has(origDate) && Has(origFrom))
                {
                    replyInfo = "<br /><br />On " + origDate + " " + origFrom + " wrote:<br />";
                }
                string newBody = Para.NewBody + replyInfo + origBody;

                var newEmail = new SendEmailRequest();
                newEmail.To = origFrom;
                newEmail.CC = newCC;
                newEmail.BCC = Para.NewBCC;
                newEmail.InReplyToId = origMessage.Id;
                newEmail.Subject = newSubject;
                newEmail.Body = newBody;
                response = await SendMessage(newEmail);
            }
            return response;
        }
        #endregion

        #region Label Management
        public async Task<ServerResponse> AddLabelToMessage(AddLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "A message Id is required.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            if (!IsAlphaNumeric(Para.Label))
            {
                throw new Exception("Invalid label: " + Sanitize(Para.Label));
            }
            System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]:Para.Label:" + Para.Label);
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/modify
            string labelId = await FindLabelId(Para.Label);
            if (!Has(labelId))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]:try to create label");
                //label doesn't exist, create it first
                labelId = await CreateLabel(Sanitize(Para.Label));
            }
            if (!Has(labelId))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]:labelId is missing");
            }
            if (Has(labelId))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]:labelId:" + labelId);
                var requestBodyObj = new Dictionary<string, string[]>()
                {
                    { "addLabelIds", new string[] { labelId } }
                };
                string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
                var message = await Post<GMailClientMessageId>($"messages/{Para.Id}/modify", jsonRequestBody);
                if (!Has(message))
                {
                    response.Message = "Unknown error 1. Could not add label.";
                }
                if (!Has(message.Id))
                {
                    response.Message = "Unknown error 2. Could not add label.";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToMessage]:success:" + message.Id);
                    response.Message = "";
                }
            }
            return response;
        }
        public async Task<ServerResponse> RemoveLabelFromMessage(RemoveLabelRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveLabelFromMessage]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "A message Id is required.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/modify
            string labelId = await FindLabelId(Para.Label);
            if (!Has(labelId))
            {
                //label doesn't exist, create it first
                labelId = await CreateLabel(Sanitize(Para.Label));
            }
            if (Has(labelId))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][RemoveLabelFromMessage]:labelId:" + labelId);
                var requestBodyObj = new Dictionary<string, string[]>()
                {
                    { "removeLabelIds", new string[] { labelId } }
                };
                string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
                var message = await Post<GMailClientMessageId>($"messages/{Para.Id}/modify", jsonRequestBody);
                if (!Has(message))
                {
                    response.Message = "Unknown error 1. Could not remove label.";
                }
                if (!Has(message.Id))
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][RemoveLabelFromMessage]:success:" + message.Id);
                    response.Message = "Unknown error 2. Could not remove label.";
                }
                else
                {
                    response.Message = "";
                }
            }
            return response;
        }
        protected async Task<string> FindLabelId(string labelName)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][FindLabelId]");
            if (!Has(labelName))
            {
                return "";
            }
            System.Diagnostics.Debug.WriteLine("[vertex][FindLabelId]:labelName:" + labelName);
            //https://developers.google.com/gmail/api/reference/rest/v1/users.labels/list
            var labels = await Get<GMailClientLabels>("labels");
            if (labels == null || labels.Labels == null)
            {
                return "";
            }
            System.Diagnostics.Debug.WriteLine("[vertex]:FindLabelId:labelName:" + labelName);
            foreach (var lbl in labels.Labels)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][FindLabelId]:lbl.Name:" + lbl.Name);
                if (labelName.Equals(lbl.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][FindLabelId]:found id:" + lbl.Id);
                    return lbl.Id;
                }
            }
            return "";
        }
        protected async Task<string> CreateLabel(string labelName)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][CreateLabel]");
            if (!Has(labelName))
            {
                System.Diagnostics.Debug.WriteLine("[vertex][CreateLabel]:labelName is missing");
                return "";
            }
            //https://developers.google.com/gmail/api/reference/rest/v1/users.labels/create
            var requestBodyObj = new Dictionary<string, string>()
            {
                { "labelListVisibility", "labelShow" },
                { "messageListVisibility", "show" },
                { "name", labelName}
            };
            System.Diagnostics.Debug.WriteLine("[vertex][CreateLabel]:labelName:" + labelName);
            string jsonRequestBody = JsonConvert.SerializeObject(requestBodyObj);
            var label = await Post<GMailClientLabel>("labels", jsonRequestBody);
            if (label == null)
            {
                return "";
            }
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:Id:" + label.Id);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:Name:" + label.Name);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:Type:" + label.Type);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:LabelListVisibility:" + label.LabelListVisibility);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:MessageListVisibility:" + label.MessageListVisibility);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:MessagesTotal:" + label.MessagesTotal);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:MessagesUnread:" + label.MessagesUnread);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:ThreadsTotal:" + label.ThreadsTotal);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:ThreadsUnread:" + label.ThreadsUnread);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:Color.TextColor:" + label.Color.TextColor);
                System.Diagnostics.Debug.WriteLine("[vertex]:CreateLabel:Color.BackgroundColor:" + label.Color.BackgroundColor);
            }
            catch (Exception) { }
            return label.Id;
        }
        public async Task<ServerResponse> AddLabelToThreadMessages(AddLabelThreadRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToThreadMessages]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "A thread Id is required.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            System.Diagnostics.Debug.WriteLine("[vertex][AddLabelToThreadMessages]Para.Id:" + Para.Id);
            //https://developers.google.com/gmail/api/reference/rest/v1/users.threads/get
            var threads = await GetThread(Para.Id);
            if (!Has(threads))
            {
                response.Message = "Invalid thread Id.";
            }
            string responses = "";
            foreach (var msg in threads.Messages)
            {
                try
                {
                    var para2 = new AddLabelRequest();
                    para2.Id = msg.Id;
                    para2.Label = Para.Label;
                    var resp = await AddLabelToMessage(para2);
                    if (resp.Message != "")
                    {
                        responses = AppendString(responses, resp.Message);
                    }
                }
                catch (Exception ex)
                {
                    responses = AppendString(responses, ex.Message);
                }
            }
            return response;
        }
        public async Task<ServerResponse> RemoveLabelFromThreadMessages(RemoveLabelThreadRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveLabelFromThreadMessages]");
            var response = new ServerResponse();
            if (!Has(Para.Id))
            {
                response.Message = "A thread Id is required.";
            }
            if (!IsAlphaNumeric(Para.Id))
            {
                throw new Exception("Invalid message Id: " + Sanitize(Para.Id));
            }
            System.Diagnostics.Debug.WriteLine("[vertex][RemoveLabelFromThreadMessages]Para.Id:" + Para.Id);
            //https://developers.google.com/gmail/api/reference/rest/v1/users.threads/get
            var threads = await GetThread(Para.Id);
            if (!Has(threads))
            {
                response.Message = "Invalid thread Id.";
            }
            string responses = "";
            foreach (var msg in threads.Messages)
            {
                try
                {
                    var para2 = new RemoveLabelRequest();
                    para2.Id = msg.Id;
                    para2.Label = Para.Label;
                    var resp = await RemoveLabelFromMessage(para2);
                    if (resp.Message != "")
                    {
                        responses = AppendString(responses, resp.Message);
                    }
                }
                catch (Exception ex)
                {
                    responses = AppendString(responses, ex.Message);
                }
            }
            return response;
        }
        #endregion
    }
}
