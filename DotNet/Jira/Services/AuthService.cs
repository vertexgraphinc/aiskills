using IdentityModel.Client;
using Jira.Constants;
using Jira.Contracts;
using Jira.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jira.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][Auth]Redeem Token - Grant: " + Para.GrantType + " - Scope: " + APIConstants.ApiScope);

            TokenResponse resp;

            if (Para.GrantType == "authorization_code")
            {
                resp = await _httpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = APIConstants.ApiTokenURL,
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
                resp = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = APIConstants.ApiTokenURL,
                    GrantType = Para.GrantType,

                    ClientId = Para.ClientId,
                    ClientSecret = Para.ClientSecret,
                    RefreshToken = Para.Code,
                });
            }
            return GetToken(resp);
        }

        OAuthToken GetToken(TokenResponse resp)
        {
            if (resp == null) {
                System.Diagnostics.Debug.WriteLine("[vertex][Jira][Auth]Get Token - Null");

                return null; 
            }       

            if (resp.IsError)
            {
                System.Diagnostics.Debug.WriteLine("[vertex][Jira][Auth]Get Token - Error: " + resp.Error);

                return new OAuthToken
                {
                    Error = resp.Error,
                    ErrorDescription = resp.HttpErrorReason
                };
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Jira][Auth]Get Token - Scope: " + resp.Scope);

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
