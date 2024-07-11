using Jira.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Jira.Constants;
using System;
using Jira.Interfaces;
namespace Jira.Controllers
{
    [ApiController, Route("[controller]")]
    public class OAuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public OAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            //if the skill is installed as a web application called "Jira" in IIS, then both URLs will work:
            //https://example.com/Jira/oauth/test
            //https://example.com/Jira/skill/oauth/test
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][OAuth]Test");

            return "hello world from oauth.";
        }

        [HttpGet("auth"), HttpGet("~/skill/{controller}/auth")]
        public void Auth()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][OAuth]Auth");

            var state = Guid.NewGuid().ToString();
            Response.Redirect($"{APIConstants.ApiAuthURL}{Request.QueryString}&state={state}&audience=api.atlassian.com");
        }

        [HttpPost("token"), HttpPost("~/skill/{controller}/token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Jira][OAuth]Redeem Token");

            return await _authService.RedeemToken(Para);
        }

    }
}
