using GMail.Contracts;
using GMail.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThreadsController : OAuthSession
    {
        [HttpPost("query")]
        public async Task<QueryEmailThreadsResponse> QueryEmailThreads(QueryEmailThreadsRequest Para)
        {
            QueryEmailThreadsResponse resp = new QueryEmailThreadsResponse
            {
                EmailThreads = null
            };

            string Token = GetSessionToken();
            if (string.IsNullOrEmpty(Token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.EmailThreads = await ListThreads(Para);

            return resp;
        }

        
    }
}
