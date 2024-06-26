﻿using MSTeams.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MSTeams.Interfaces;
using MSTeams.Constants;

namespace MSTeams.Controllers
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
            string qs = Request.QueryString.ToString();
            qs = qs.Replace("&prompt=consent", "");
            Response.Redirect(APIConstants.GraphApiAuthURL + $"common/oauth2/v2.0/authorize{qs}");
        }

        [HttpPost("token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            return await _authService.RedeemToken(Para);
        }
    }
}
