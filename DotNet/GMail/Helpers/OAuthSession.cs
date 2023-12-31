﻿using GMail.Contracts;
using GMail.GMailClient;
using Google.Apis.Auth.OAuth2;
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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Google.Apis.Requests.BatchRequest;

namespace GMail.Helpers
{
    public class OAuthSession : ValidationHelpers
    {
        int _defaultMaxResults = 5;
        public string GetSessionToken()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;
            
            string Auth = Request.Headers["Authorization"].First<string>();
            if (!Has(Auth))
                return null;
            string[] ps = Auth.Split(' ');
            if (ps.Length == 1)
                return Auth;

            if (string.Compare(ps[0], "Bearer", true) == 0)
                return ps[1];
            return Auth;
        }
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
                    //s = s.Replace("\r", "").Replace("\n", "");

                    System.Diagnostics.Debug.WriteLine("[vertex][Get<T>]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Get<T>]:s:" + Sanitize(s));

                    return JsonConvert.DeserializeObject<T>(s);
                }
            }
        }     
        public async Task<T> Post<T>(string Url, string content)
        {
            if (!Url.StartsWith("http"))
                Url = $"https://www.googleapis.com/gmail/v1/users/me/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Post, Url);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(s);
                }
            }
        }

        protected string EncodeBodyText(string body)
        {
            body = Convert.ToBase64String(Encoding.UTF8.GetBytes(body));
            body = body.Replace('+', '-').Replace('/', '_').Replace("=", "");
            return body;
        }
        protected string DecodeBase64String(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return "";
            }
            try
            {
                byte[] data = WebEncoders.Base64UrlDecode(base64String);
                base64String = System.Text.Encoding.UTF8.GetString(data);
                return base64String + "\n\n";
            }
            catch (Exception ex)
            {
                return "\n==============================================\nERROR: " + ex.Message + "\n==============================================\n";
            }
        }
        public async Task<GMailClientMessageFull> GetMessage(string Id)
        {
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/get
            return await Get<GMailClientMessageFull>($"messages/{Id}?format=full");
        }
        public async Task<GMailClientThread> GetThread(string Id)
        {
            //https://developers.google.com/gmail/api/reference/rest/v1/users.messages/get
            return await Get<GMailClientThread>($"threads/{Id}");
        }
    }
}
