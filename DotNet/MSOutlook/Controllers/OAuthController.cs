using MSOutlook.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MSOutlook.Interfaces;
using MSOutlook.Constants;

namespace MSOutlook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public OAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("auth")]
        public void Auth()
        {
            string tenant = "common";
            Response.Redirect(APIConstants.GraphApiAuthURL + $"{tenant}/oauth2/v2.0/authorize{Request.QueryString}");
        }

        [HttpPost("token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            return await _authService.RedeemToken(Para);
        }
    }
}
