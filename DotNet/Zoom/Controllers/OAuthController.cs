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
            string url = APIConstants.ZoomApiAuthURL + $"authorize?client_id={Request.Query["client_id"]}&response_type={Request.Query["response_type"]}&redirect_uri={Request.Query["redirect_uri"]}";
            Response.Redirect(APIConstants.ZoomApiAuthURL + $"authorize?client_id={Request.Query["client_id"]}&response_type={Request.Query["response_type"]}&redirect_uri={Request.Query["redirect_uri"]}");
        }

        [HttpPost("token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            return await _authService.RedeemToken(Para);
        }
    }
}
