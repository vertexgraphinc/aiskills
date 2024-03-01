using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Slack.Helpers
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
                Url = $"https://slack.com/api/{Url}";

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
                Url = $"https://slack.com/api/{Url}";

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
    }
}
