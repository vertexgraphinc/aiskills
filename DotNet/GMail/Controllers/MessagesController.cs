using GMail.Contracts;
using GMail.GMailClient;
using GMail.Helpers;
using Google.Apis.Gmail.v1;
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
    public class MessagesController : OAuthSession
    {
        [HttpPost("query")]
        public async Task<QueryEmailsResponse> QueryEmails(QueryEmailsRequest Para)
        {
            QueryEmailsResponse resp = new QueryEmailsResponse
            {
                EmailMessages = null
            };

            string Token = GetSessionToken();
            if (string.IsNullOrEmpty(Token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.EmailMessages = await ListMessage(Para);

            return resp;
        }

        
    }
}
