using MSTeams.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSTeams.Interfaces
{
    public interface IChatService
    {
        Task<List<ChatResponse>> QueryChats(ChatsQueryRequest request, string token);

        Task<ChatResponse> GetChat(ChatGetRequest request, string token);

        Task<bool> CreateChat(ChatCreateRequest request, string token);

        Task<bool> UpdateChat(ChatUpdateRequest request, string token);

        Task<List<MemberResponse>> QueryChatMembers(ChatMembersQueryRequest request, string token);

        Task<MemberResponse> GetChatMember(ChatMemberGetRequest request, string token);

        Task<bool> AddChatMember(ChatMemberAddRequest request, string token);

        Task<bool> RemoveChatMember(ChatMemberRemoveRequest request, string token);

        Task<List<MessageResponse>> QueryChatMessages(ChatMessagesQueryRequest request, string token);

        Task<MessageResponse> GetChatMessage(ChatMessageGetRequest request, string token);

        Task<bool> SendChatMessage(ChatMessageSendRequest request, string token);

        Task<bool> UpdateChatMessage(ChatMessageUpdateRequest request, string token);

        Task<bool> RemoveChatMessage(ChatMessageRemoveRequest request, string token);
    }
}
