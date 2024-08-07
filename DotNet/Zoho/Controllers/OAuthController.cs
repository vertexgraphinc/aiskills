using Zoho.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Zoho.Constants;
using System;
using Zoho.Interfaces;

namespace Zoho.Controllers
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

        [HttpGet("test")]
        [HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Zoho][OAuth]Test");

            return "Hello World from OAuth.";
        }

        [HttpGet("auth")]
        [HttpGet("~/skill/{controller}/auth")]
        public void Auth()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Zoho][OAuth]Auth");

            var state = Guid.NewGuid().ToString();
            Response.Redirect($"{ApiConstants.ApiAuthURL}{Request.QueryString}&state={state}&audience=api.atlassian.com");
        }

        [HttpPost("token"), HttpPost("~/skill/{controller}/token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Zoho][OAuth]Redeem Token");

            return await _authService.RedeemToken(Para);
        }

    }
}