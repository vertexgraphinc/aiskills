using IdentityModel.Client;
using MSTeams.Constants;
using MSTeams.Contracts;
using MSTeams.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSTeams.Services
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
            string tenant = "common";

            if (Para.GrantType == "authorization_code")
            {
                resp = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = APIConstants.GraphApiAuthURL + $"{tenant}/oauth2/v2.0/token",
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    Code = Para.Code,
                    RedirectUri = Para.RedirectUri,
                    Parameters =
                        {
                            { "scope", APIConstants.GraphApiScope }
                        }
                });
            }
            else
            {
                resp = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = APIConstants.GraphApiAuthURL + $"{tenant}/oauth2/v2.0/token",
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
                System.Diagnostics.Debug.WriteLine("[vertex][GetToken]ex:" + ex.ToString());
                return null;
            }
        }

        #endregion

    }
}
