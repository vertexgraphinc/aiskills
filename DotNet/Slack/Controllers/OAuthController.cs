using Slack.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using Slack.Constants;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.DataProtection;
namespace Slack.Controllers
{
    [ApiController,Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        [HttpGet("test"),HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "Slack" in IIS, then both URLs will work:
            //https://example.com/Slack/oauth/test
            //https://example.com/Slack/skill/oauth/test
            System.Diagnostics.Debug.WriteLine("[vertex][OAuth]Test");
            return "hello world from oauth.";
        }

        [HttpGet("auth"),HttpGet("~/skill/{controller}/auth")]
        public void Auth()
        {
            var queryString = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            if (!string.IsNullOrEmpty(queryString["scope"]))
            {
                queryString["user_scope"] = queryString["scope"];
                queryString.Remove("scope");
            }
            Response.Redirect($"{APIConstants.ApiBaseURL}?{queryString}");
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
                    }
                });
                return GetToken(resp);
            }
            else
            {
                try
                {
                    resp = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = APIConstants.ApiAuthURL,
                        GrantType = Para.GrantType,

                        ClientId = Para.ClientId,
                        ClientSecret = Para.ClientSecret,
                        RefreshToken = Para.Code
                    });
                }
                catch(System.Exception ex)
                {
                    return new OAuthToken
                    {
                        Error = resp.Error,
                        ErrorDescription = ex.Message
                    };
                }
                return GetRefreshToken(resp);
            }
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
            string resp2 = resp.Json.ToString();
            try
            {
                var jresp = JsonSerializer.Deserialize<OAuthAuthUserResponse>(resp2);
                return new OAuthToken
                {
                    AccessToken = jresp.AuthedUser.AccessToken,
                    RefreshToken = jresp.AuthedUser.RefreshToken,
                    ExpiresIn = jresp.AuthedUser.ExpiresIn.ToString()
                };
            }
            catch(System.Exception ex)
            {
                return new OAuthToken
                {
                    Error = resp.Error,
                    ErrorDescription = ex.Message
                };
            }
        }

        OAuthToken GetRefreshToken(TokenResponse resp)
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
            string resp2 = resp.Json.ToString(); 
            try
            {
                var jresp = JsonSerializer.Deserialize<OAuthRefreshTokenResponse>(resp2);
                return new OAuthToken
                {
                    AccessToken = jresp.AccessToken,
                    RefreshToken = jresp.RefreshToken,
                    ExpiresIn = jresp.ExpiresIn.ToString()
                };
            }
            catch (System.Exception ex)
            {
                return new OAuthToken
                {
                    Error = resp.Error,
                    ErrorDescription = ex.Message
                };
            }
        }

    }
}
