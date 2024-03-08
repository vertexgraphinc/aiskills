using MSTeams.Contracts;
using MSTeams.DTOs;
using MSTeams.Helpers;
using MSTeams.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private async Task<MSGraphChats> QueryChatsRawData(ChatsQueryRequest request, string token)
        {
            string query = "$filter=";

            string lastUpdatedBeginTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-7));
            string lastUpdatedEndTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
            if (!string.IsNullOrEmpty(request.Topic))
            {
                query += $"contains(topic, \'{request.Topic}\')";
            }

            if (!string.IsNullOrEmpty(request.ChatType))
            {
                if (query != "$filter=")
                {
                    query += " and ";
                }
                query += $"chatType eq \'{request.ChatType}\'";
            }

            if (!string.IsNullOrEmpty(request.LastUpdatedBeginTime))
            {
                DateTime BeginDT = DateTime.Parse(request.LastUpdatedBeginTime);
                lastUpdatedBeginTime = UtilityHelper.FormatDateTimeUtc(BeginDT);
            }

            if (!string.IsNullOrEmpty(request.LastUpdatedEndTime))
            {
                DateTime EndDT = DateTime.Parse(request.LastUpdatedEndTime);
                lastUpdatedEndTime = UtilityHelper.FormatDateTimeUtc(EndDT);
            }

            if (query != "$filter=")
            {
                query += " and ";
            }
            query += $"lastUpdatedDateTime ge {lastUpdatedBeginTime} and lastUpdatedDateTime le {lastUpdatedEndTime}";
            query = "me/chats?$expand=members&" + query;

            MSGraphChats chats = await _apiHelper.Get<MSGraphChats>(query, token);

            if (!string.IsNullOrEmpty(request.MemberEmails))
            {
                string[] emails = request.MemberEmails.Replace(" ", "").Split(',');
                chats.Value = chats.Value.Where(chat =>
                {
                    bool hasAllEmails = true;
                    string memberEmails = string.Join(",", chat.Members.Select(member => member.Email));
                    foreach (string email in emails)
                    {
                        hasAllEmails = hasAllEmails && memberEmails.Contains(email);
                    }
                    return hasAllEmails;
                }).ToList();
            }
            return chats;
        }

        public async Task<List<ChatResponse>> QueryChats(ChatsQueryRequest request, string token)
        {
            try
            {
                MSGraphChats chats = await QueryChatsRawData(request, token);
                if (chats == null || chats.Value == null)
                    return new List<ChatResponse>();

                return chats.Value.Select(chat => new ChatResponse
                {
                    Topic = chat.Topic,
                    ChatType = chat.ChatType,
                    MemberEmails = chat.Members != null && chat.Members.Any() ? string.Join(",", chat.Members.Select(member => member.Email)) : "",
                    LastUpdated = chat.LastUpdatedDateTime,
                    WebUrl = chat.WebUrl
                }).ToList();
            }
            catch (HttpRequestException e)
            {
                return new List<ChatResponse>();
            }
        }

        public async Task<bool> CreateChat(ChatCreateRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.ChatType) || string.IsNullOrEmpty(request.MemberEmails))
                return false;

            try
            {
                MSGraphUserEntity creator = await _apiHelper.Get<MSGraphUserEntity>("me", token);
                if (string.IsNullOrEmpty(creator.Id))
                    return false;

                List<MemberRequestBody> members = request.MemberEmails.Replace(" ", "").Split(",").Select(email => new MemberRequestBody
                {
                    ODataType = "#microsoft.graph.aadUserConversationMember",
                    Roles = new List<string>
                    {
                        "owner"
                    },
                    UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{email}\')"
                }).Append(new MemberRequestBody
                {
                    ODataType = "#microsoft.graph.aadUserConversationMember",
                    Roles = new List<string>
                    {
                        "owner"
                    },
                    UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{creator.Id}\')"
                }).ToList();

                string urlQuery = $"chats";
                object body = new
                {
                    topic = request.Topic,
                    chatType = request.ChatType,
                    members
                };

                return await _apiHelper.Post<bool>(urlQuery, body, token);
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateChats(ChatUpdateRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.UpdatedTopic))
                return false;

            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                var tasks = chats.Value.Where(chat => chat.ChatType == "group").Select(async chat =>
                {
                    string urlQuery = $"chats/{chat.Id}";
                    object body = new
                    {
                        topic = request.UpdatedTopic
                    };
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<List<MemberResponse>> QueryChatMembers(ChatMembersQueryRequest request, string token)
        {
            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return new List<MemberResponse>();

                return chats.Value.SelectMany(chat => chat.Members.Where(member => 
                    (string.IsNullOrEmpty(request.DisplayName) || member.DisplayName == request.DisplayName) && 
                    (string.IsNullOrEmpty(request.role) || member.Roles.Any(role => role == request.role))
                ).Select(member => new MemberResponse
                {
                    Email = member.Email,
                    DisplayName = member.DisplayName,
                    Roles = member.Roles,
                    GroupTopic = chat.Topic,
                    VisibleHistoryStart = member.VisibleHistoryStartDateTime
                })).ToList();
            }
            catch (HttpRequestException e)
            {
                return new List<MemberResponse>();
            }
        }

        public async Task<bool> AddChatMember(ChatMemberAddRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Email))
                return false;

            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                var tasks = chats.Value.Where(chat => chat.ChatType == "group").Select(async chat =>
                {
                    string urlQuery = $"chats/{chat.Id}/members";
                    MemberRequestBody body = new MemberRequestBody
                    {
                        ODataType = "#microsoft.graph.aadUserConversationMember",
                        Roles = new List<string>
                        {
                            "owner"
                        },
                        UserODataBind = "https://graph.microsoft.com/v1.0/users(" + $"\'{request.Email}\')",
                        VisibleHistoryStartDateTime = "0001-01-01T00:00:00Z"
                    };
                    return await _apiHelper.Post<bool>(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveChatMember(ChatMemberRemoveRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Email))
                return false;

            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                var tasks = chats.Value.Where(chat => chat.ChatType == "group").SelectMany(chat => chat.Members.Where(member => member.Email == request.Email).Select(async member =>
                {
                    string urlQuery = $"chats/{chat.Id}/members/{member.Id}";
                    return await _apiHelper.Delete(urlQuery, token);
                }));
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<List<MessageResponse>> QueryChatMessages(ChatMessagesQueryRequest request, string token)
        {
            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return new List<MessageResponse>();

                string query = "$filter=";

                string lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-1));
                string lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
                if (!string.IsNullOrEmpty(request.LastModifiedBeginTime))
                {
                    DateTime BeginDT = DateTime.Parse(request.LastModifiedBeginTime);
                    lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(BeginDT);
                }
                if (!string.IsNullOrEmpty(request.LastModifiedEndTime))
                {
                    DateTime EndDT = DateTime.Parse(request.LastModifiedEndTime);
                    lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(EndDT);
                }
                query += $"lastModifiedDateTime gt {lastModifiedBeginTime} and lastModifiedDateTime lt {lastModifiedEndTime}";

                var tasks = chats.Value.Select(async chat =>
                {
                    string urlQuery = $"me/chats/{chat.Id}/messages?" + query;
                    MSGraphMessages messages = await _apiHelper.Get<MSGraphMessages>(urlQuery, token);
                    messages.GroupTopic = chat.Topic;
                    return messages;
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any())
                    return new List<MessageResponse>();

                return results.SelectMany(result => result.Value.Where(message => message.DeletedDateTime == null && message.From != null && message.From.User != null && (!string.IsNullOrEmpty(message.From.User.Email) || !string.IsNullOrEmpty(message.From.User.DisplayName))).Where(message => 
                    string.IsNullOrEmpty(request.From) || 
                    (message.From != null && message.From.User != null && (!string.IsNullOrEmpty(message.From.User.Email) ? message.From.User.Email == request.From : (!string.IsNullOrEmpty(message.From.User.DisplayName) && message.From.User.DisplayName == request.From)))
                ).Select(message => new MessageResponse
                {
                    GroupTopic = result.GroupTopic,
                    From = message.From != null && message.From.User != null ? !string.IsNullOrEmpty(message.From.User.Email) ? message.From.User.Email : (!string.IsNullOrEmpty(message.From.User.DisplayName) ? message.From.User.DisplayName : "") : "",
                    Content = message.Body != null ? message.Body.Content : "",
                    Created = message.CreatedDateTime,
                    LastModified = message.LastModifiedDateTime
                })).ToList();
            }
            catch (HttpRequestException e)
            {
                return new List<MessageResponse>();
            }
        }

        public async Task<bool> SendChatMessages(ChatMessageSendRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Content))
                return false;

            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                object body = new
                {
                    body = new
                    {
                        content = request.Content
                    }
                };
                var tasks = chats.Value.Select(async chat =>
                {
                    string urlQuery = $"chats/{chat.Id}/messages";
                    return await _apiHelper.Post<bool>(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateChatMessages(ChatMessageUpdateRequest request, string token)
        {
            if (string.IsNullOrEmpty(request.Content))
               return false;

            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                string query = "$filter=";

                string lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-1));
                string lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
                if (!string.IsNullOrEmpty(request.LastModifiedBeginTime))
                {
                    DateTime BeginDT = DateTime.Parse(request.LastModifiedBeginTime);
                    lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(BeginDT);
                }
                if (!string.IsNullOrEmpty(request.LastModifiedEndTime))
                {
                    DateTime EndDT = DateTime.Parse(request.LastModifiedEndTime);
                    lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(EndDT);
                }
                query += $"lastModifiedDateTime gt {lastModifiedBeginTime} and lastModifiedDateTime lt {lastModifiedEndTime}";

                var tasks1 = chats.Value.Select(async chat =>
                {
                    string urlQuery = $"chats/{chat.Id}/messages?" + query;
                    MSGraphMessages messages = await _apiHelper.Get<MSGraphMessages>(urlQuery, token);
                    messages.GroupTopic = chat.Topic;
                    return messages;
                });
                var results1 = await Task.WhenAll(tasks1);
                if (results1 == null || !results1.Any())
                    return false;

                MSGraphUserEntity user = await _apiHelper.Get<MSGraphUserEntity>("me", token);
                object body = new
                {
                    body = new
                    {
                        content = request.Content
                    }
                };
                var tasks2 = results1.SelectMany(result => result.Value.Where(message => message.DeletedDateTime == null && message.From != null && message.From.User != null && (!string.IsNullOrEmpty(message.From.User.Email) || !string.IsNullOrEmpty(message.From.User.DisplayName))).Where(message =>
                    (message.From != null && message.From.User != null && (!string.IsNullOrEmpty(message.From.User.DisplayName) ? message.From.User.DisplayName == user.DisplayName : message.From.User.Email == (string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail)))
                )).Select(async message =>
                {
                    string urlQuery = $"chats/{message.ChatId}/messages/{message.Id}";
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results2 = await Task.WhenAll(tasks2);
                if (results2 == null || !results2.Any() || !results2.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveChatMessages(ChatMessageRemoveRequest request, string token)
        {
            try
            {
                MSGraphChats chats = await QueryChatsRawData(new ChatsQueryRequest
                {
                    Topic = request.Topic,
                    ChatType = request.ChatType,
                    MemberEmails = request.MemberEmails,
                    LastUpdatedBeginTime = request.LastUpdatedBeginTime,
                    LastUpdatedEndTime = request.LastUpdatedEndTime
                }, token);
                if (chats == null || chats.Value == null)
                    return false;

                string query = "$filter=";

                string lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-1));
                string lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
                if (!string.IsNullOrEmpty(request.LastModifiedBeginTime))
                {
                    DateTime BeginDT = DateTime.Parse(request.LastModifiedBeginTime);
                    lastModifiedBeginTime = UtilityHelper.FormatDateTimeUtc(BeginDT);
                }
                if (!string.IsNullOrEmpty(request.LastModifiedEndTime))
                {
                    DateTime EndDT = DateTime.Parse(request.LastModifiedEndTime);
                    lastModifiedEndTime = UtilityHelper.FormatDateTimeUtc(EndDT);
                }
                query += $"lastModifiedDateTime gt {lastModifiedBeginTime} and lastModifiedDateTime lt {lastModifiedEndTime}";

                var tasks1 = chats.Value.Select(async chat =>
                {
                    string urlQuery = $"chats/{chat.Id}/messages?" + query;
                    MSGraphMessages messages = await _apiHelper.Get<MSGraphMessages>(urlQuery, token);
                    messages.GroupTopic = chat.Topic;
                    return messages;
                });
                var results1 = await Task.WhenAll(tasks1);
                if (results1 == null || !results1.Any())
                    return false;

                MSGraphUserEntity user = await _apiHelper.Get<MSGraphUserEntity>("me", token);
                var tasks2 = results1.SelectMany(result => result.Value.Where(message => message.DeletedDateTime == null && message.From != null && message.From.User != null && (!string.IsNullOrEmpty(message.From.User.Email) || !string.IsNullOrEmpty(message.From.User.DisplayName))).Where(message =>
                    (message.From != null && (message.From.User != null && (!string.IsNullOrEmpty(message.From.User.DisplayName) ? message.From.User.DisplayName == user.DisplayName : message.From.User.Email == (string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail))))
                )).Select(async message =>
                {
                    string urlQuery = $"users/{user.Id}/chats/{message.ChatId}/messages/{message.Id}/softDelete";
                    return await _apiHelper.Post<bool>(urlQuery, null, token);
                });
                var results2 = await Task.WhenAll(tasks2);
                if (results2 == null || !results2.Any() || !results2.All(result => result == true))
                    return false;

                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }
    }
}
