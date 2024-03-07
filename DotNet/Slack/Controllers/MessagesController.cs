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
    public class MessagesController : MessageHelpers
    {
        [HttpGet("test"), HttpGet("~/skill/{controller}/test")]
        public string Test()
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages]Test");
            return "Messages controller test.";
        }

        #region Send Message to Channel
        [HttpPost("send_to_channel"), HttpPost("~/skill/{controller}/send_to_channel")]
        public async Task<ServerResponse> SendMessageToChannel(SendMessageRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMessage]");
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
                response = await SendMessage(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMessage]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Send Message to User
        [HttpPost("send_to_user"), HttpPost("~/skill/{controller}/send_to_user")]
        public async Task<ServerResponse> SendMessageToUser(SendMessageRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMessage]");
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
                Para.Channel = await FindUserIdByName(Para.Channel);
                if(Para.Channel == null)
                {
                    Response.StatusCode = 500;
                    response.Message = "Username not found.";
                    return response;
                }
                response = await SendMessage(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMessage]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Send Message
        [HttpPost("dnd"), HttpPost("~/skill/{controller}/dnd")]
        public async Task<ServerResponse> SetDnd(SetDndRequest Para)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SetDnd]");
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
                response = await SetDndTime(Para);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMessage]response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region List Channel Messages
        [HttpPost("list/channel"), HttpPost("~/skill/{controller}/list/channel")]
        public async Task<ListChannelMsgsResponse> ListChannelMessages(ListChannelMsgsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListChannelMessages]");
            var response = new ListChannelMsgsResponse();
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
                response = await GetChannelMessagesByName(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListChannelMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Search Messages
        [HttpPost("search"), HttpPost("~/skill/{controller}/search")]
        public async Task<SearchMessagesResponse> SearchMessages(SearchRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SearchMessages]");
            var response = new SearchMessagesResponse();
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
                response = await QueryMessages(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SearchMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region Set Custom Status
        [HttpPost("status"), HttpPost("~/skill/{controller}/status")]
        public async Task<ServerResponse> SetCustomStatus(SetUserStatusRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SetCustomStatus]");
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
                response = await SetUserStatus(request);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SetCustomStatus] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region List All Messages
        [HttpPost("list/all"), HttpPost("~/skill/{controller}/list/all")]
        public async Task<ListAllMsgsResponse> ListAllMessages(ListAllMsgsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllMessages]");
            var response = new ListAllMsgsResponse();
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
                response = await GetAllChannels(request, "all");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region List All Direct Messages
        [HttpPost("list/all_dms"), HttpPost("~/skill/{controller}/list/all_dms")]
        public async Task<ListAllMsgsResponse> ListAllDirectMessages(ListAllMsgsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllDirectMessages]");
            var response = new ListAllMsgsResponse();
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
                response = await GetAllChannels(request, "im");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllDirectMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region List All Group Messages
        [HttpPost("list/all_group"), HttpPost("~/skill/{controller}/list/all_group")]
        public async Task<ListAllMsgsResponse> ListAllGroupMessages(ListAllMsgsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllGroupMessages]");
            var response = new ListAllMsgsResponse();
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
                response = await GetAllChannels(request, "mpim");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllGroupMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

        #region List All Channel Messages
        [HttpPost("list/all_channel"), HttpPost("~/skill/{controller}/list/all_channel")]
        public async Task<ListAllMsgsResponse> ListAllChannelMessages(ListAllMsgsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllChannelMessages]");
            var response = new ListAllMsgsResponse();
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
                response = await GetAllChannels(request, "public_channel,private_channel");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                response.Message = ex.Message;
                return response;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListAllChannelMessages] Response:" + JsonConvert.SerializeObject(response));

            return response;
        }
        #endregion

    }
}
