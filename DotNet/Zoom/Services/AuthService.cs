using IdentityModel.Client;
using Zoom.Constants;
using Zoom.Contracts;
using Zoom.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zoom.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        #region Token
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            TokenResponse resp = null;

            if (Para.GrantType == "authorization_code")
            {
                resp = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = APIConstants.ZoomApiAuthURL + $"token",
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    Code = Para.Code,
                    RedirectUri = Para.RedirectUri,
                    Parameters =
                    {
                        { "grant_type", Para.GrantType },
                        { "code", Para.Code },
                        { "redirect_uri", Para.RedirectUri }
                    }
                });
            }
            else
            {
                resp = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = APIConstants.ZoomApiAuthURL + $"token",
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    RefreshToken = Para.RefreshToken,
                    Parameters =
                    {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", Para.RefreshToken }
                    }
                });
            }

            return GetToken(resp);
        }

        OAuthToken GetToken(TokenResponse resp)
        {
            try
            {
                if (resp == null)
                {
                    return null;
                }

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
            catch (Exception ex)
            {
                string exc = ex.Message;
                return null;
            }
        }

        #endregion

    }
}
