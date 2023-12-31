﻿using Microsoft.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Interfaces;
using Microsoft.Constants;

namespace Microsoft.Controllers
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
            string tenant = "organizations";
            Response.Redirect(APIConstants.GraphApiAuthURL + $"{tenant}/oauth2/v2.0/authorize{Request.QueryString}");
        }

        [HttpPost("token")]
        public async Task<OAuthToken> RedeemToken(OAuthTokenPara Para)
        {
            return await _authService.RedeemToken(Para);
        }
    }
}
