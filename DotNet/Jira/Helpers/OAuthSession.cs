using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jira.Contracts;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Jira.Helpers
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

        public async Task<JiraSiteInfo> GetJiraSiteInfo()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.atlassian.com/oauth/token/accessible-resources");

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("[vertex][Get<T>]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Get<T>]:s:" + Sanitize(s));

                    List<JiraSiteInfo> siteInfoResp = JsonConvert.DeserializeObject<List<JiraSiteInfo>>(s);

                    return siteInfoResp[0];

                }
            }
        }
        public async Task<T> Get<T>(string Url)
        {
            
            var siteInfo = await GetJiraSiteInfo();
            var apiUrl = $"https://api.atlassian.com/ex/jira/{siteInfo.Id}/rest/api/3/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

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

        public async Task<bool> Get(string Url)
        {
            var siteInfo = await GetJiraSiteInfo();
            var apiUrl = $"https://api.atlassian.com/ex/jira/{siteInfo.Id}/rest/api/3/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception(httpResponse.ReasonPhrase);
                    }

                    System.Diagnostics.Debug.WriteLine("[vertex][Get]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Get]:s:" + Sanitize(s));

                    return httpResponse.IsSuccessStatusCode;
                }
            }
        }

        public async Task<bool> Post(string Url, string content)
        {
            var siteInfo = await GetJiraSiteInfo();
            var apiUrl = $"https://api.atlassian.com/ex/jira/{siteInfo.Id}/rest/api/3/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception(httpResponse.ReasonPhrase);
                    }

                    System.Diagnostics.Debug.WriteLine("[vertex][Post]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Post]:s:" + Sanitize(s));

                    return httpResponse.IsSuccessStatusCode;
                }
            }
        }


        public async Task<bool> Put(string Url, string content)
        {
            var siteInfo = await GetJiraSiteInfo();
            var apiUrl = $"https://api.atlassian.com/ex/jira/{siteInfo.Id}/rest/api/3/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception(httpResponse.ReasonPhrase);
                    }

                    System.Diagnostics.Debug.WriteLine("[vertex][Put]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Put]:s:" + Sanitize(s));

                    return httpResponse.IsSuccessStatusCode;
                }
            }
        }


        public async Task<T> Post<T>(string Url, string content)
        {
            var siteInfo = await GetJiraSiteInfo();
            var apiUrl = $"https://api.atlassian.com/ex/jira/{siteInfo.Id}/rest/api/3/{Url}";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            string Token = GetSessionToken();
            request.Headers.Add("Authorization", "Bearer " + Token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                using (var httpResponse = await client.SendAsync(request))
                {
                    string s = await httpResponse.Content.ReadAsStringAsync();

                    if (!httpResponse.IsSuccessStatusCode)
                    {
                        throw new Exception(httpResponse.ReasonPhrase);
                    }

                    System.Diagnostics.Debug.WriteLine("[vertex][Post]:Url:" + Url);
                    System.Diagnostics.Debug.WriteLine("[vertex][Post]:s:" + Sanitize(s));

                    return JsonConvert.DeserializeObject<T>(s);
                }
            }
        }

        
    }
}
