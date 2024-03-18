using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MSTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("query")]
        public async Task<ChatsQueryResponse> QueryChats(ChatsQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Query]");
            ChatsQueryResponse resp = new ChatsQueryResponse
            {
                Chats = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Chats = await _chatService.QueryChats(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Query]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("create")]
        public async Task<ServerResponse> CreateChat(ChatCreateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Create]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isCreated = await _chatService.CreateChat(request, token);
                if (isCreated)
                {
                    resp.Message = "Chat created successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to create chat.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Create]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("update")]
        public async Task<ServerResponse> UpdateChats(ChatUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Update]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isUpdated = await _chatService.UpdateChats(request, token);
                if (isUpdated)
                {
                    resp.Message = "Chat updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update chat.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][Update]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/query")]
        public async Task<ChatMembersQueryResponse> QueryChatMembers(ChatMembersQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][QueryMembers]");
            ChatMembersQueryResponse resp = new ChatMembersQueryResponse
            {
                Members = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Members = await _chatService.QueryChatMembers(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][QueryMembers]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/add")]
        public async Task<ServerResponse> AddChatMember(ChatMemberAddRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][AddMember]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isAdded = await _chatService.AddChatMember(request, token);
                if (isAdded)
                {
                    resp.Message = "Chat member added successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to add chat member.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][AddMember]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("members/remove")]
        public async Task<ServerResponse> RemoveChatMember(ChatMemberRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][RemoveMembers]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isRemoved = await _chatService.RemoveChatMember(request, token);
                if (isRemoved)
                {
                    resp.Message = "Chat member removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove chat member.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][RemoveMembers]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("messages/query")]
        public async Task<ChatMessagesQueryResponse> QueryChatMessages(ChatMessagesQueryRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][QueryMessages]");
            ChatMessagesQueryResponse resp = new ChatMessagesQueryResponse
            {
                Messages = null
            };
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                resp.Messages = await _chatService.QueryChatMessages(request, token);
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][QueryMessages]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("messages/send")]
        public async Task<ServerResponse> SendChatMessages(ChatMessageSendRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][SendMessages]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isSent = await _chatService.SendChatMessages(request, token);
                if (isSent)
                {
                    resp.Message = "Chat message sent successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to send chat message.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][SendMessages]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("messages/update")]
        public async Task<ServerResponse> UpdateChatMessages(ChatMessageUpdateRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][UpdateMessages]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isUpdated = await _chatService.UpdateChatMessages(request, token);
                if (isUpdated)
                {
                    resp.Message = "Chat message updated successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to update chat message.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][UpdateMessages]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("messages/remove")]
        public async Task<ServerResponse> RemoveChatMessages(ChatMessageRemoveRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Chats][RemoveMessages]");
            var resp = new ServerResponse();
            Response.StatusCode = 200;

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                resp.Message = "Unauthorized.";
                return resp;
            }

            try
            {
                bool isRemoved = await _chatService.RemoveChatMessages(request, token);
                if (isRemoved)
                {
                    resp.Message = "Chat message removed successfully.";
                }
                else
                {
                    Response.StatusCode = 400;
                    resp.Message = "Failed to remove chat message.";
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                resp.Message = e.Message;
                return resp;
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Chats][RemoveMessages]response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }
    }
}
