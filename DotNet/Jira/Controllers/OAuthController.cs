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
/*        eyJraWQiOiJhdXRoLmF0bGFzc2lhbi5jb20tQUNDRVNTLWE5Njg0YTZlLTY4MjctNGQ1Yi05MzhjLWJkOTZjYzBiOTk0ZCIsImFsZyI6IlJTMjU2In0.eyJqdGkiOiJhMzg0ZWFiMi01ZGQ1LTRmYmMtYjA2Yi01ZjRlNGZiMGZmNjgiLCJzdWIiOiI3MTIwMjA6YTkxODQ3MzAtNGU4OC00Nzc4LTgzODUtODhlMDZmMzRhYTc2IiwibmJmIjoxNzExOTkxODE0LCJpc3MiOiJodHRwczovL2F1dGguYXRsYXNzaWFuLmNvbSIsImlhdCI6MTcxMTk5MTgxNCwiZXhwIjoxNzExOTk1NDE0LCJhdWQiOiJ0aGNCakhQc3hQbUo3ek1yMHNUV0dMWmNrN01MQzdVbSIsImNsaWVudF9pZCI6InRoY0JqSFBzeFBtSjd6TXIwc1RXR0xaY2s3TUxDN1VtIiwiaHR0cHM6Ly9pZC5hdGxhc3NpYW4uY29tL3Nlc3Npb25faWQiOiJlYjAxZDAyMi1lZTM4LTRkNjgtYWZlNy05MTZiMDk1ZGZjNjAiLCJodHRwczovL2lkLmF0bGFzc2lhbi5jb20vYXRsX3Rva2VuX3R5cGUiOiJBQ0NFU1MiLCJodHRwczovL2F0bGFzc2lhbi5jb20vZmlyc3RQYXJ0eSI6ZmFsc2UsImh0dHBzOi8vYXRsYXNzaWFuLmNvbS92ZXJpZmllZCI6dHJ1ZSwiaHR0cHM6Ly9hdGxhc3NpYW4uY29tL3N5c3RlbUFjY291bnRJZCI6IjcxMjAyMDowNWJmMDNjMC1iYmViLTQ3YWUtYmVlMC1kNzUwZGE2OTA5ZmUiLCJodHRwczovL2lkLmF0bGFzc2lhbi5jb20vcHJvY2Vzc1JlZ2lvbiI6InVzLWVhc3QtMSIsImh0dHBzOi8vYXRsYXNzaWFuLmNvbS9vYXV0aENsaWVudElkIjoidGhjQmpIUHN4UG1KN3pNcjBzVFdHTFpjazdNTEM3VW0iLCJodHRwczovL2F0bGFzc2lhbi5jb20vc3lzdGVtQWNjb3VudEVtYWlsIjoiMDMxODVmODktNjRmOS00M2IwLWE4NGMtMmE0OTNlYWU2ZDE4QGNvbm5lY3QuYXRsYXNzaWFuLmNvbSIsImh0dHBzOi8vYXRsYXNzaWFuLmNvbS8zbG8iOnRydWUsInNjb3BlIjoicmVhZDpqaXJhLXdvcmsgbWFuYWdlOmppcmEtd2ViaG9vayB3cml0ZTpqaXJhLXdvcmsiLCJodHRwczovL2lkLmF0bGFzc2lhbi5jb20vdmVyaWZpZWQiOnRydWUsImh0dHBzOi8vaWQuYXRsYXNzaWFuLmNvbS91anQiOiI3MjU3YjgzMy02YWNhLTQxMWItYmIyNS0xZDI2MzZmMzQwN2IiLCJodHRwczovL2F0bGFzc2lhbi5jb20vc3lzdGVtQWNjb3VudEVtYWlsRG9tYWluIjoiY29ubmVjdC5hdGxhc3NpYW4uY29tIiwiaHR0cHM6Ly9hdGxhc3NpYW4uY29tL2VtYWlsRG9tYWluIjoidmVydGV4Z3JhcGguY29tIn0.VD5sdTapGgISTY_NvbT2Fs314gBVAdzWt24IYcYUDX5AO-tPf-tjZ0kwGfJDZ5DTeoMsGc5AKsvrk8C40GUvmsea3i7DtbUXul7-WpRdb-fyJhCNQHgP_c1XxBcfYChisa7FMMxZAgadDHOCYeeUOQreBbX78tuodr5JrP6xreksvpv_lHD5YaNH3vHmMlVrnMbNOE9oLsoaSFWDYBeTchidOiWYpp6AMBqYHV3-aJF0AFxp-RPUWCQb71DyZDJnnkhcQRmHVY_kzS2j1wcZwA7jfVJ_uEyurxFodhIR0gTdLkkVSqJJU2Q56gaZuvJWaM260EjkS2-Vb0G6XYd_0w
*/        OAuthToken GetToken(TokenResponse resp)
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
