using MSTeams.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSTeams.Interfaces
{
    public interface IChatService
    {
        Task<List<ChatResponse>> QueryChats(ChatsQueryRequest request, string token);

        Task<bool> CreateChat(ChatCreateRequest request, string token);

        Task<bool> UpdateChats(ChatUpdateRequest request, string token);

        Task<List<MemberResponse>> QueryChatMembers(ChatMembersQueryRequest request, string token);

        Task<bool> AddChatMember(ChatMemberAddRequest request, string token);

        Task<bool> RemoveChatMember(ChatMemberRemoveRequest request, string token);

        Task<List<MessageResponse>> QueryChatMessages(ChatMessagesQueryRequest request, string token);

        Task<bool> SendChatMessages(ChatMessageSendRequest request, string token);

        Task<bool> UpdateChatMessages(ChatMessageUpdateRequest request, string token);

        Task<bool> RemoveChatMessages(ChatMessageRemoveRequest request, string token);
    }
}
