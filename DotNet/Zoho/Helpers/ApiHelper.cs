using System.Net.Http;
using System.Threading.Tasks;
using Zoho.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;

public static class ApiHelper
{
    public static HttpClient CreateHttpClient(IHttpClientFactory httpClientFactory, string authorizationHeader)
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(ApiConstants.ApiBaseURL);
        client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

        return client;
    }

    public static string BuildQueryString(string query)
    {
        if (!string.IsNullOrEmpty(query) && query.Contains(":"))
        {
            query = query.Replace(":", "=");
        }

        return string.IsNullOrEmpty(query) ? string.Empty : "?" + query;
    }

    public static async Task<IActionResult> SendHttpRequest(HttpClient client, HttpMethod method, string url, HttpContent content = null)
    {
        HttpResponseMessage response;
        if (method == HttpMethod.Get)
        {
            response = await client.GetAsync(url);
        }
        else if (method == HttpMethod.Post)
        {
            response = await client.PostAsync(url, content);
        }
        else if (method == HttpMethod.Patch)
        {
            response = await client.PatchAsync(url, content);
        }
        else
        {
            throw new NotSupportedException($"HTTP method {method} is not supported.");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return new OkObjectResult(responseContent);
        }

        return new ObjectResult(responseContent)
        {
            StatusCode = (int)response.StatusCode
        };
    }

    public static string GetAuthorizationHeader(IHttpContextAccessor httpContextAccessor)
    {
        string authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            throw new UnauthorizedAccessException("Authorization header is missing.");
        }

        return authorizationHeader.Replace("Bearer", "Zoho-oauthtoken ");
    }
}