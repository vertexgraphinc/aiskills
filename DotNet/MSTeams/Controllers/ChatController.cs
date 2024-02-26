using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MSTeams.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("query"), HttpPost("~/skill/{controller}/query")]
        public async Task<ChatsQueryResponse> QueryChats(ChatsQueryRequest request)
        {
            ChatsQueryResponse resp = new ChatsQueryResponse
            {
                Chats = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.Chats = await _chatService.QueryChats(request, token);
            return resp;
        }

        [HttpPost("create"), HttpPost("~/skill/{controller}/create")]
        public async Task<IActionResult> CreateChat(ChatCreateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isCreated = await _chatService.CreateChat(request, token);
            if (isCreated)
            {
                return Ok("Chat created successfully.");
            }
            else
            {
                return BadRequest("Failed to create chat.");
            }
        }

        [HttpPost("update"), HttpPost("~/skill/{controller}/update")]
        public async Task<IActionResult> UpdateChats(ChatUpdateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isUpdated = await _chatService.UpdateChats(request, token);
            if (isUpdated)
            {
                return Ok("Chat updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update chat.");
            }
        }

        [HttpPost("queryMembers"), HttpPost("~/skill/{controller}/members/query")]
        public async Task<ChatMembersQueryResponse> QueryChatMembers(ChatMembersQueryRequest request)
        {
            ChatMembersQueryResponse resp = new ChatMembersQueryResponse
            {
                Members = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.Members = await _chatService.QueryChatMembers(request, token);
            return resp;
        }

        [HttpPost("addMember"), HttpPost("~/skill/{controller}/members/add")]
        public async Task<IActionResult> AddChatMember(ChatMemberAddRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isAdded = await _chatService.AddChatMember(request, token);
            if (isAdded)
            {
                return Ok("Chat member added successfully.");
            }
            else
            {
                return BadRequest("Failed to add chat member.");
            }
        }

        [HttpPost("removeMember"), HttpPost("~/skill/{controller}/members/remove")]
        public async Task<IActionResult> RemoveChatMember(ChatMemberRemoveRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isRemoved = await _chatService.RemoveChatMember(request, token);
            if (isRemoved)
            {
                return Ok("Chat member removed successfully.");
            }
            else
            {
                return BadRequest("Failed to remove chat member.");
            }
        }

        [HttpPost("queryMessages"), HttpPost("~/skill/{controller}/messages/query")]
        public async Task<ChatMessagesQueryResponse> QueryChatMessages(ChatMessagesQueryRequest request)
        {
            ChatMessagesQueryResponse resp = new ChatMessagesQueryResponse
            {
                Messages = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.Messages = await _chatService.QueryChatMessages(request, token);
            return resp;
        }

        [HttpPost("sendMessage"), HttpPost("~/skill/{controller}/messages/send")]
        public async Task<IActionResult> SendChatMessage(ChatMessageSendRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent = await _chatService.SendChatMessages(request, token);
            if (isSent)
            {
                return Ok("Chat message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send chat message.");
            }
        }

        [HttpPost("updateMessage"), HttpPost("~/skill/{controller}/messages/update")]
        public async Task<IActionResult> UpdateChatMessage(ChatMessageUpdateRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isUpdated = await _chatService.UpdateChatMessages(request, token);
            if (isUpdated)
            {
                return Ok("Chat message updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update chat message.");
            }
        }

        [HttpPost("removeMessage"), HttpPost("~/skill/{controller}/messages/remove")]
        public async Task<IActionResult> RemoveChatMessage(ChatMessageRemoveRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isRemoved = await _chatService.RemoveChatMessages(request, token);
            if (isRemoved)
            {
                return Ok("Chat message removed successfully.");
            }
            else
            {
                return BadRequest("Failed to remove chat message.");
            }
        }
    }
}
