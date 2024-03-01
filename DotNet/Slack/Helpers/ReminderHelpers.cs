using Newtonsoft.Json;
using Slack.Contracts;
using Slack.Helpers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net.Http;

namespace Slack.Helpers
{
    public class ReminderHelpers : OAuthSession
    {
        public async Task<ServerResponse> CreateReminder(AddReminderRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][AddReminder]");
            var response = new ServerResponse();

            var result = await Post<ApiResult>($"/reminders.add", JsonConvert.SerializeObject(Para, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            if (result.Ok == true)
            {
                response.Message = "Reminder successfully added";
            }
            else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;

        }
        
    }
}
