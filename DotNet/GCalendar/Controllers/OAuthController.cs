﻿using GCalendar.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace GCalendar.Controllers
{
    [ApiController,Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        [HttpGet("test"),HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "gcalendar" in IIS, then both URLs will work:
            //https://example.com/gcalendar/oauth/test
            //https://example.com/gcalendar/skill/oauth/test
            System.Diagnostics.Debug.WriteLine("[vertex][OAuth]Test");
            return "hello world from oauth.";
        }

        [HttpGet("auth"),HttpGet("~/skill/{controller}/auth")]
        public void Auth()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][OAuth]Auth");
            Response.Redirect($"https://accounts.google.com/o/oauth2/auth{Request.QueryString}");
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
                    Address = "https://oauth2.googleapis.com/token",
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    Code = Para.Code,
                    RedirectUri = Para.RedirectUri,
                    Parameters =
                        {

                            { "scope", "https://www.googleapis.com/auth/calendar" }
                        }
                });
            }
            else
            {
                resp = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = "https://oauth2.googleapis.com/token",
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    RefreshToken = Para.Code
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
