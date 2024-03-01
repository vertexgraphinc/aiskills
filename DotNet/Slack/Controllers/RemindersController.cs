using Slack.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using Newtonsoft.Json;
using Slack.Helpers;
using System;

namespace Slack.Controllers
{
    [ApiController, Route("[controller]")]
    public class RemindersController : ReminderHelpers
    {
        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Reminders]Test");
            return "hello world from reminders.";
        }

        #region Add Reminder
        [HttpPost("add"), HttpPost("~/skill/{controller}/add")]
        public async Task<ServerResponse> AddReminder(AddReminderRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][AddReminder]");
            var response = new ServerResponse();
            Response.StatusCode = 200;

            string Token = GetSessionToken();
            if (!Has(Token))
            {
                Response.StatusCode = 401;
                response.Message = "Unauthorized.";
                return response;
            }
            try
            {
                response = await CreateReminder(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][AddReminder]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

       

    }
}
