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
    public class MessageHelpers : OAuthSession
    {

        private Dictionary<string, string> userMap = new Dictionary<string, string>();

        public async Task<ServerResponse> SendMessage(SendMessageRequest Para)
        {
            var channelId = await FindChannelIdByName(Para.ChannelName);
            var response = new ServerResponse();

            var message = new Message
            {
                Channel = channelId,
                Text = Para.Text,
            };

            var result = await Post<ApiResult>($"/chat.postMessage", JsonConvert.SerializeObject(message, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            if(result.Ok == true) 
            {
                response.Message = "Message successfully sent";
            } else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;

        }

        public async Task<ServerResponse> SetDndTime(SetDndRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SetDnd]");
            var response = new ServerResponse();

            var result = await Post<ApiResult>($"/dnd.setSnooze", JsonConvert.SerializeObject(Para, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddMinutes(int.Parse(Para.NumOfMins));
            if (result.Ok == true)
            {
                response.Message = "Notifications paused till " + currentTime.ToString("MMM dd 'at' h:mmtt");
            }
            else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;

        }

        public async Task<ServerResponse> AddReminder(AddReminderRequest Para)
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
                response.Message = "Reminder successfully added "; 
            }
            else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;

        }

        public async Task<ListChannelMsgsResponse> GetChannelMessages(ListChannelMsgsRequest Para)
        {

            var response = new ListChannelMsgsResponse
            {
                Channel = Para.Channel
            };

            Para.Channel = await FindChannelIdByName(Para.Channel);

            Para.Oldest = ParseDateToUnix(Para.Oldest);

            var result = await Post<HistoryResponse>($"/conversations.history", JsonConvert.SerializeObject(Para, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            

            if (result.Ok == true)
            {
                
                foreach(var message in result.Messages)
                {
                    message.Timestamp = ParseUnixToDate(double.Parse(message.Timestamp));
                    message.User = await GetUser(message.User);
                }
                response.Messages= result.Messages;

            }
            else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;

        }

        public async Task<string> FindChannelIdByName(string channelName)
        {
            
            var response = await Get<ChannelsResponse>($"/conversations.list");
            var channel = response?.Channels.FirstOrDefault(c => c.Name == channelName);

            return channel?.Id;
        }

        public async Task<string> GetUser(string userId)
        {
            if (!userMap.ContainsKey(userId))
            {
                var response = await Get<UserInfoResponse>($"/users.info?user={userId}");
                if (response.Ok == true)
                {
                    userMap.Add(response.User.Id, response.User.RealName);
                }

                return response.User.RealName;
            } 
            
            return userMap.GetValueOrDefault(userId);
            
        } 
    }
}
