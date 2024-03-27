using Zoom.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zoom.Interfaces;
using Zoom.Constants;

namespace Zoom.Controllers
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
            Response.Redirect(APIConstants.ZoomApiAuthURL + $"authorize{Request.QueryString}");
        }

        [HttpPost("token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            return await _authService.RedeemToken(Para);
        }
    }
}
