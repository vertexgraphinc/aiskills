using GMail.Contracts;
using GMail.Helpers;
using GMail.GMailClient;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Google.Apis.Requests.BatchRequest;

namespace GMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThreadsController : MessagesHelpers
    {
        [HttpPost("query")]
        [HttpPost("~/skill/{controller}/query")]
        public async Task<QueryThreadsResponse> QueryThreads(SearchFilters Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Threads][QueryThreads]");
            var resp = new QueryThreadsResponse();

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
            }
            try
            {
                resp.Messages = await ListThreads(Para);
                if (resp.Messages.Count == 0)
                {
                    resp.Message = "No threaded messages were found.";
                }
                else
                {
                    resp.Message = $"Found {resp.Messages.Count} threaded messages(s).";
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                resp.Message = Sanitize(StripHtmlTags( ex.ToString()));
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Threads][QueryThreads]response:" + JsonConvert.SerializeObject(resp));

            return resp;
        }

        [HttpPost("add_label_to_thread")]
        [HttpPost("~/skill/{controller}/add_label_to_thread")]
        public async Task<ServerResponse> AddLabelToThread(AddLabelThreadRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Threads][AddLabelToThread]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
            }
            try
            {
                if (IsJsonObject(Para.Id))
                {
                    var msgs = GetIdsFromMessageObj(Para.Id);
                    string errors = "";
                    foreach (var msg in msgs)
                    {
                        Para.Id = msg.Id;
                        var resp = await AddLabelToThreadMessages(Para);
                        if (resp != null && resp.Message == "")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success";
                    }
                }
                else
                {
                    var resp = await AddLabelToThreadMessages(Para);
                    if (resp != null && resp.Message == "")
                    {
                        response.Message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Threads][AddLabelToThread]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        [HttpPost("remove_label_from_thread")]
        [HttpPost("~/skill/{controller}/remove_label_from_thread")]
        public async Task<ServerResponse> RemoveLabelFromThread(RemoveLabelThreadRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Threads][RemoveLabelFromThread]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
            }
            try
            {
                if (IsJsonObject(Para.Id))
                {
                    var msgs = GetIdsFromMessageObj(Para.Id);
                    string errors = "";
                    foreach (var msg in msgs)
                    {
                        Para.Id = msg.Id;
                        var resp = await RemoveLabelFromThreadMessages(Para);
                        if (resp != null && resp.Message == "")
                        {
                            errors += resp.Message;
                        }
                    }
                    if (errors == "")
                    {
                        response.Message = "Success";
                    }
                }
                else
                {
                    var resp = await RemoveLabelFromThreadMessages(Para);
                    if (resp != null && resp.Message == "")
                    {
                        response.Message = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Threads][RemoveLabelFromThread]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
