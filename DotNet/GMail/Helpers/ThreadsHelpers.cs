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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace GMail.Helpers
{
    public class ThreadsHelpers : ExpandoHelpers
    {
        int _defaultMaxResults = 5;
        public async Task<ExpandoObject> GetMessage(string Id)
        {
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/get
            return await Get<ExpandoObject>($"messages/{Id}?format=full");
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
                                    var message = new GMailThread();
                                    message.Id = GetStringVal(props["id"]);
                                    message.ThreadId = GetStringVal(props["threadId"]);
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
    }
}