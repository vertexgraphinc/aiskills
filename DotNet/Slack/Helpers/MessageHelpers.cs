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
        private int? userTimeZoneOffset = null;

        public async Task<ServerResponse> SendMessage(SendMessageRequest Para)
        {
            var response = new ServerResponse();

            var result = await Post<ApiResult>($"/chat.postMessage", JsonConvert.SerializeObject(Para, Formatting.None,
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

        public async Task<ServerResponse> SetUserStatus(SetUserStatusRequest Para)
        {
            var response = new ServerResponse();

            var status = new SlackProfileData
            {
                Profile = new ProfileData
                {
                    StatusText = Para.StatusText,
                    StatusEmoji = Para.StatusEmoji,
                    StatusExpiration = ParseDateToUnix(Para.StatusExpiration),
                }
            };

            var result = await Post<ApiResult>($"/users.profile.set", JsonConvert.SerializeObject(status, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            if (result.Ok == true)
            {
                response.Message = "Custom profile successfully set";
            }
            else
            {
                Response.StatusCode = 500;
                response.Message = result.Error;
            }

            return response;
        }


        public async Task<SearchMessagesResponse> QueryMessages(SearchRequest Para)
        {
            var result = await Get<SearchMessagesResponse>($"/search.messages?query={Para.Query}&count=10");

            if (result.Ok == true)
            {
                result.Message = "Successfully retrieved results";
                foreach ( var message in result.Messages.Matches ) {
                    if(message.Type == "im")
                    {
                        message.Channel.Name = await GetUser(message.Channel.Name);
                    }
                    message.Timestamp = ParseUnixToDate(double.Parse(message.Timestamp), await GetUserTimeZoneOffset());
                }
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = result.Error;
            }

            return result;

        }

        public async Task<SearchFilesResponse> QueryFiles(SearchRequest Para)
        {
            var result = await Get<SearchFilesResponse>($"/search.files?query={Para.Query}&count=10");

            if (result.Ok == true)
            {
                result.Message = "Successfully retrieved results";
                foreach (var file in result.Files.Matches)
                {
                    file.User = await GetUser(file.User);
                    file.Timestamp = ParseUnixToDate(double.Parse(file.Timestamp), await GetUserTimeZoneOffset());
                }
            }
            else
            {
                Response.StatusCode = 500;
                result.Message = result.Error;
            }

            return result;

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

        public async Task<ListChannelMsgsResponse> GetChannelMessagesByName(ListChannelMsgsRequest Para)
        {

            var response = new ListChannelMsgsResponse
            {
                Channel = Para.Channel
            };
            
            if (Para.IsDM != null && Para.IsDM.ToLower() == "true")
            {
                Para.Channel = await FindUserDMByName(Para.Channel);
            }
            else
            {
                Para.Channel = await FindChannelIdByName(Para.Channel);
            }

            if(Para.Limit == 0) Para.Limit = 10; 

            Para.Oldest = ParseDateToUnix(Para.Oldest);
            Para.Latest = ParseDateToUnix(Para.Latest);
            

            var result = await Post<HistoryResponse>($"/conversations.history", JsonConvert.SerializeObject(Para, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            

            if (result.Ok == true)
            {
                
                foreach(var message in result.Messages)
                {
                    message.Timestamp = ParseUnixToDate(double.Parse(message.Timestamp), await GetUserTimeZoneOffset());
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


        public async Task<ListChannelMsgsResponse> GetChannelMessagesById(ListChannelMsgsRequest Para)
        {

            var response = new ListChannelMsgsResponse
            {
                Channel = Para.Channel
            };
            

            var result = await Post<HistoryResponse>($"/conversations.history", JsonConvert.SerializeObject(Para, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            if (result.Ok == true)
            {

                foreach (var message in result.Messages)
                {
                    message.Timestamp = ParseUnixToDate(double.Parse(message.Timestamp), await GetUserTimeZoneOffset());
                    message.User = await GetUser(message.User);
                }
                response.Messages = result.Messages;
                response.Message = "Returned " + result.Messages.Count + " messages";

            }
            else
            {
                response.Messages = null;
            }

            return response;

        }





        public async Task<ListAllMsgsResponse> GetAllChannels(ListAllMsgsRequest Para, string types)
        {

            var response = new ListAllMsgsResponse
            {
                Channels = new List<ListChannelMsgsResponse>()
            };


            if (types == "all") types = "public_channel,private_channel,mpim,im";

            var result = await Get<ChannelsResponse>($"/conversations.list?types={types}");

            if (result.Ok == true)
            {
                Para.Oldest = ParseDateToUnix(Para.Oldest);
                Para.Latest = ParseDateToUnix(Para.Latest);

                foreach (var channel in result.Channels)
                {
                    var request = new ListChannelMsgsRequest
                    {
                        Oldest = Para.Oldest,
                        Latest = Para.Latest,
                        Channel = channel.Id,
                        Limit = Para.Limit == 0 ? 5 : Para.Limit
                    };
                    
                    var channelMessages = await GetChannelMessagesById(request);
                    channelMessages.Channel = channel.Name;
                    if (channel.IsDM == true) channelMessages.Channel = "DM's with " + await GetUser(channel.UserId);
                    
                    if (channelMessages.Messages != null) response.Channels.Add(channelMessages);
                }

                response.Message = "Successfully retrieved messages for " + response.Channels.Count + " channels";
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
            
            var response = await Get<ChannelsResponse>($"/conversations.list?types=public_channel,private_channel");
            var channel = response?.Channels.FirstOrDefault(c => c.Name == channelName);

            return channel?.Id;
        }

        public async Task<string> FindUserIdByName(string username)
        {

            var response = await Get<UserListResponse>($"/users.list");
            var user = response?.Members?.FirstOrDefault(u => (u.Name.ToLower() == username.ToLower()
                || u.RealName.ToLower() == username.ToLower()));

            return user?.Id;
        }

        public async Task<string> FindUserDMByName(string username)
        {
            var id = await FindUserIdByName(username);
            if(id == null) return null;

            var response = await Get<ChannelsResponse>($"/conversations.list?types=im");
            var channel = response?.Channels.FirstOrDefault(c => c.UserId == id);

            return channel?.Id;
        }

        public async Task<int> GetUserTimeZoneOffset()
        {
            if (userTimeZoneOffset == null)
            {
                var response = await Get<SlackUserData>($"/openid.connect.userInfo");
                if (response.Ok == false) return 0;

                var userData = await Get<UserInfoResponse>($"/users.info?user={response.UserId}");
                if (userData.Ok == false) return 0;

                userTimeZoneOffset = userData.User.TimeZoneOffset;
            }

            return (int)userTimeZoneOffset;

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
