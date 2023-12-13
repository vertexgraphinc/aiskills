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
    public class ThreadsController : MessagesHelpers
    {
        [HttpPost("query")]
        public async Task<QueryEmailThreadsResponse> QueryEmailThreads(SearchFilters Para)
        {
            var resp = new QueryEmailThreadsResponse
            {
                EmailThreads = null
            };

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                resp.MessageFromServer = "Unauthorized.";
            }
            try
            {
                resp.EmailThreads = await ListThreads(Para);
                if (resp.EmailThreads.Count == 0)
                {
                    resp.MessageFromServer = "No emails were found.";
                }
                else
                {
                    resp.MessageFromServer = $"Top {resp.EmailThreads.Count} threads returned.";
                }
            }
            catch (Exception ex)
            {
                resp.MessageFromServer = ex.Message;
            }
            return resp;
        }
    }
}
