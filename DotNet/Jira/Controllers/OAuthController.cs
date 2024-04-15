using Jira.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using Jira.Constants;
using Newtonsoft.Json.Linq;
using System;
namespace Jira.Controllers
{
    [ApiController,Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        [HttpGet("test"),HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "Jira" in IIS, then both URLs will work:
            //https://example.com/Jira/oauth/test
            //https://example.com/Jira/skill/oauth/test
            System.Diagnostics.Debug.WriteLine("[vertex][OAuth]Test");
            return "hello world from oauth.";
        }

        [HttpGet("auth"),HttpGet("~/skill/{controller}/auth")]
        public void Auth()
        {
            var state = Guid.NewGuid().ToString();
            Response.Redirect($"{APIConstants.ApiBaseURL}{Request.QueryString}&state={state}&audience=api.atlassian.com");
        }

        [HttpPost("token"),HttpPost("~/skill/{controller}/token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][OAuth]RedeemToken");
            var client = new HttpClient();
            TokenResponse resp = null;

            if (Para.GrantType == "authorization_code")
            {
                resp = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = APIConstants.ApiAuthURL,
                    GrantType = Para.GrantType,
                   
                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    Code = Para.Code,
                    RedirectUri = Para.RedirectUri,
                    Parameters =
                    {
                        { "scope", APIConstants.ApiScope } 
                    },
                    
                });
            }
            else
            {
                resp = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = APIConstants.ApiAuthURL,
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    RefreshToken = Para.Code,
                    Parameters =
                    {
                        { "scope", APIConstants.ApiScope }
                    }
                });

            }
            return GetToken(resp);
        }
        OAuthToken GetToken(TokenResponse resp)
        {
            if (resp == null)
                return null;

            if (resp.IsError)
            {
                return new OAuthToken
                {
                    Error = resp.Error,
                    ErrorDescription = resp.HttpErrorReason
                };
            }

            return new OAuthToken
            {
                AccessToken = resp.AccessToken,
                RefreshToken = resp.RefreshToken,
                ExpiresIn = resp.ExpiresIn.ToString(),
                Scope = resp.Scope
            };
        }


    }
}
