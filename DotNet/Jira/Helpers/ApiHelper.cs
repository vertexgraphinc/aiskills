using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Jira.Constants;
using Jira.DTOs;

namespace Jira.Helpers
{
    public class ApiHelper
    {
        private readonly HttpClient _httpClient;

        public ApiHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JiraSiteInfo> GetJiraSiteInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, APIConstants.ApiInfoUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][GetSiteInfo]:s:" + UtilityHelper.Sanitize(s));

                List<JiraSiteInfo> siteInfoResp = JsonConvert.DeserializeObject<List<JiraSiteInfo>>(s);
                return siteInfoResp[0];
            }
        }

        public async Task<T> Get<T>(string url, string token)
        {
            var siteInfo = await GetJiraSiteInfo(token);
            var apiUrl = $"{APIConstants.ApiBaseURL}{siteInfo.Id}{APIConstants.ApiRestURL}{url}";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();
                //s = s.Replace("\r", "").Replace("\n", "");

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Get<T>]:Url:" + url);
                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Get<T>]:s:" + UtilityHelper.Sanitize(s));

                return JsonConvert.DeserializeObject<T>(s);
            }
        }

        public async Task<bool> Get(string url, string token)
        {
            var siteInfo = await GetJiraSiteInfo(token);
            var apiUrl = $"{APIConstants.ApiBaseURL}{siteInfo.Id}{APIConstants.ApiRestURL}{url}";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json;odata=verbose");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Get]:Url:" + url);
                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Get]:s:" + UtilityHelper.Sanitize(s));

                return httpResponse.IsSuccessStatusCode;
            }
        }

        public async Task<bool> Post(string url, string content, string token)
        {
            var siteInfo = await GetJiraSiteInfo(token);
            var apiUrl = $"{APIConstants.ApiBaseURL}{siteInfo.Id}{APIConstants.ApiRestURL}{url}";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json;odata=verbose");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Post]:Url:" + url);
                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Post]:s:" + UtilityHelper.Sanitize(s));

                return httpResponse.IsSuccessStatusCode;
            }
        }

        public async Task<bool> Put(string url, string content, string token)
        {
            var siteInfo = await GetJiraSiteInfo(token);
            var apiUrl = $"{APIConstants.ApiBaseURL}{siteInfo.Id}{APIConstants.ApiRestURL}{url}";

            var request = new HttpRequestMessage(HttpMethod.Put, apiUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json;odata=verbose");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Put]:Url:" + url);
                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Put]:s:" + UtilityHelper.Sanitize(s));

                return httpResponse.IsSuccessStatusCode;
            }
        }

        public async Task<T> Post<T>(string url, string content, string token)
        {
            var siteInfo = await GetJiraSiteInfo(token);
            var apiUrl = $"{APIConstants.ApiBaseURL}{siteInfo.Id}{APIConstants.ApiRestURL}{url}";

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json;odata=verbose");
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponse.ReasonPhrase);
                }

                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Post<T>]:Url:" + url);
                System.Diagnostics.Debug.WriteLine("[vertex][JiraIssues][Post<T>]:s:" + UtilityHelper.Sanitize(s));

                return JsonConvert.DeserializeObject<T>(s);
            }
        }

    }
}
