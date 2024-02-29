using Newtonsoft.Json;
using MSTeams.Constants;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MSTeams.Helpers
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
            if (!url.StartsWith(APIConstants.GraphApiBaseURL))
            {
                url = APIConstants.GraphApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
        }

        public async Task<T> Post<T>(string url, object requestBody, string token)
        {
            if (!url.StartsWith(APIConstants.GraphApiBaseURL))
            {
                url = APIConstants.GraphApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            if (requestBody != null)
            {
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                request.Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            }

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.Accepted)
                    {
                        return typeof(T) == typeof(bool) ? (T)(object)true : default(T);
                    }

                    if (httpResponse.Content.Headers.ContentLength == 0)
                    {
                        return default(T);
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
            if (!url.StartsWith(APIConstants.GraphApiBaseURL))
            {
                //if (!url.StartsWith("users"))
                //{
                //    url = APIConstants.GraphApiBaseURL + url;
                //} 
                //else
                //{
                //    url = APIConstants.GraphApiBaseURL.Remove(APIConstants.GraphApiBaseURL.LastIndexOf("me/")) + url;
                //}
                url = APIConstants.GraphApiBaseURL + url;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var httpResponse = await _httpClient.SendAsync(request))
            {
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
            if (!url.StartsWith(APIConstants.GraphApiBaseURL))
            {
                url = APIConstants.GraphApiBaseURL + url;
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
