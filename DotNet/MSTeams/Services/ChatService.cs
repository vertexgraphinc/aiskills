using MSTeams.Contracts;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSTeams.Services
{
    public class ChatService : IChatService
    {
        private readonly ApiHelper _apiHelper;

        public ChatService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<ChatResponse>> QueryChats(ChatsQueryRequest request, string token)
        {

        }

        public async Task<bool> CreateChat(ChatCreateRequest request, string token)
        {

        }

        public async Task<bool> UpdateChats(ChatUpdateRequest request, string token)
        {

        }

        public async Task<List<MemberResponse>> QueryChatMembers(ChatMembersQueryRequest request, string token)
        {

        }

        public async Task<bool> AddChatMember(ChatMemberAddRequest request, string token)
        {

        }

        public async Task<bool> RemoveChatMember(ChatMemberRemoveRequest request, string token)
        {

        }

        public async Task<List<MessageResponse>> QueryChatMessages(ChatMessagesQueryRequest request, string token)
        {

        }

        public async Task<bool> SendChatMessages(ChatMessageSendRequest request, string token)
        {

        }

        public async Task<bool> UpdateChatMessages(ChatMessageUpdateRequest request, string token)
        {

        }

        public async Task<bool> RemoveChatMessages(ChatMessageRemoveRequest request, string token)
        {

        }
    }
}
