using Newtonsoft.Json;
using Salesforce.Constants;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Salesforce.Helpers
{
    public class ApiHelper
    {
        private readonly HttpClient _httpClient;

        public ApiHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(string url, string token)
        {
            if (!url.StartsWith(APIConstants.SalesforceApiBaseURL))
            {
                url = APIConstants.SalesforceApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                { 
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                else
                {
                    // Handle error response, log it or throw an exception as appropriate
                    throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode}");
                }
            }
        }

        public async Task<T> Post<T>(string url, object requestBody, string token)
        {
            if (!url.StartsWith(APIConstants.SalesforceApiBaseURL))
            {
                url = APIConstants.SalesforceApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            if (requestBody != null)
            {
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            }

            using (var httpResponse = await _httpClient.SendAsync(request))
            {

                string s = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    List<HttpStatusCode> successStatusCodes = new List<HttpStatusCode> { HttpStatusCode.Accepted, HttpStatusCode.NoContent, HttpStatusCode.Created };
                    if (successStatusCodes.Contains(httpResponse.StatusCode))
                    {
                        return typeof(T) == typeof(bool) ? (T)(object)true : default(T);
                    }

                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        return default(T);
                    }

                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                else
                {
                    // Handle error response, log it or throw an exception as appropriate
                    throw new HttpRequestException($"Request failed with status code {httpResponse.StatusCode}");
                }
            }
        }

        public async Task<bool> Delete(string url, string token)
        {
            if (!url.StartsWith(APIConstants.SalesforceApiBaseURL))
            {
                url = APIConstants.SalesforceApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Handle error response, log it or throw an exception as appropriate
                    // You might want to return false or throw a specific exception depending on your application's needs
                    return false;
                }
            }
        }

        public async Task<bool> Patch(string url, object requestBody, string token)
        {
            if (!url.StartsWith(APIConstants.SalesforceApiBaseURL))
            {
                url = APIConstants.SalesforceApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            if (requestBody != null)
            {
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            }

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string s = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Handle error response, log it or throw an exception as appropriate
                    // You might want to return false or throw a specific exception depending on your application's needs
                    return false;
                }
            }
        }
    }
}
