using Microsoft.AspNetCore.Mvc;
using MSTeams.Contracts;
using MSTeams.Interfaces;
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

        [HttpPost("query")]
        public async Task<ChatsQueryResponse> QueryChats(ChatsQueryRequest request)
        {

        }

        [HttpPost("get")]
        public async Task<ChatResponse> GetChat(ChatGetRequest request)
        {

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChat(ChatCreateRequest request)
        {

        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateChat(ChatUpdateRequest request)
        {

        }

        [HttpPost("queryMembers")]
        public async Task<ChatMembersQueryResponse> QueryChatMembers(ChatMembersQueryRequest request)
        {

        }

        [HttpPost("getMember")]
        public async Task<MemberResponse> GetChatMember(ChatMemberGetRequest request)
        {

        }

        [HttpPost("addMember")]
        public async Task<IActionResult> AddChatMember(ChatMemberAddRequest request)
        {

        }

        [HttpPost("removeMember")]
        public async Task<IActionResult> RemoveChatMember(ChatMemberRemoveRequest request)
        {

        }

        [HttpPost("queryMessages")]
        public async Task<ChatMessagesQueryResponse> QueryChatMessages(ChatMessagesQueryRequest request)
        {

        }

        [HttpPost("getMessage")]
        public async Task<MessageResponse> GetChatMessage(ChatMessageGetRequest request)
        {

        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendChatMessage(ChatMessageSendRequest request)
        {

        }

        [HttpPost("updateMessage")]
        public async Task<IActionResult> UpdateChatMessage(ChatMessageUpdateRequest request)
        {

        }

        [HttpPost("removeMessage")]
        public async Task<IActionResult> RemoveChatMessage(ChatMessageRemoveRequest request)
        {

        }
    }
}
