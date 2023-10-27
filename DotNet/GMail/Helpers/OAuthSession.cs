using GMail.Contracts;
using GMail.GMailClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GMail.Helpers
{
    public class OAuthSession: ControllerBase
    {
        public string GetSessionToken()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;
            
            string Auth = Request.Headers["Authorization"].First<string>();
            if (string.IsNullOrEmpty(Auth))
                return null;
            string[] ps = Auth.Split(' ');
            if (ps.Length == 1)
                return Auth;

            if (string.Compare(ps[0], "Bearer", true) == 0)
                return ps[1];
            return Auth;
        }

        #region messages
        public async Task<List<GMailMessage>> ListMessage(QueryEmailsRequest Para)
        {
            List<GMailMessage> RetMsgs = new List<GMailMessage>();
            DateTime Begin = DateTime.Now.AddDays(-1);
            DateTime End = DateTime.Now;
            if (!string.IsNullOrEmpty(Para.BeginTime))
            {
                Begin = DateTime.Parse(Para.BeginTime);
            }
            if (!string.IsNullOrEmpty(Para.EndTime))
            {
                End = DateTime.Parse(Para.EndTime);
            }

            string q = $"q=in:sent after:{Begin.ToShortDateString()} before:{End.ToShortDateString()}";

            GMailClientMessages msgs =  await Get<GMailClientMessages>($"messages?{q}");
            if (msgs == null)
                return RetMsgs;

            foreach (GMailClientMessage msg in msgs.Messages)
            {
                GMailClientMessage dmsg = await GetMessage(msg.Id);
                if (dmsg == null)
                    continue;

                string Subject = "";
                if(dmsg.Payload != null && dmsg.Payload.Headers != null)
                {
                    foreach(GMailClientHeader h in dmsg.Payload.Headers)
                    {
                        if(string.Compare(h.Name, "Subject", true) ==0)
                        {
                            Subject = h.Value;
                            break;
                        }
                    }
                }
                RetMsgs.Add(new GMailMessage
                {
                    Id = dmsg.Id,
                    Subject = Subject,
                    Body = dmsg.Snippet
                });
            }

            return RetMsgs;
        }

        public async Task<GMailClientMessage> GetMessage(string Id)
        {
            return await Get<GMailClientMessage>($"messages/{Id}");
        }
        #endregion

        #region threads
        public async Task<List<GMailThread>> ListThreads(QueryEmailThreadsRequest Para)
        {
            List<GMailThread> RetThreads = new List<GMailThread>();

            DateTime Begin = DateTime.Now.AddDays(-1);
            DateTime End = DateTime.Now;
            if (!string.IsNullOrEmpty(Para.BeginTime))
            {
                Begin = DateTime.Parse(Para.BeginTime);
            }
            if (!string.IsNullOrEmpty(Para.EndTime))
            {
                End = DateTime.Parse(Para.EndTime);
            }

            string q = $"q=in:sent after:{Begin.ToShortDateString()} before:{End.ToShortDateString()}";
            GMailClientThreads threads = await Get<GMailClientThreads>($"threads?{q}");
            if (threads == null)
                return RetThreads;

            foreach(GMailClientThread t in threads.Threads)
            {
                GMailThread mt = new GMailThread
                {
                    Id = t.Id,
                    Subject = t.Snippet
                };

                RetThreads.Add(mt);
            }


            return RetThreads;
        }
        #endregion

        public async Task<T> Get<T>(string Url)
        {
            if (!Url.StartsWith("http"))
                Url = $"https://www.googleapis.com/gmail/v1/users/me/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Get, Url);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(s);
                }
            }
        }
    }
}
