using MSOutlook.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSOutlook.Interfaces
{
    public interface IMailService
    {
        Task<List<EmailResponse>> ListMessage(QueryEmailsRequest request, string token);

        Task<EmailResponse> GetMessage(EmailIdRequest request, string token);

        Task<bool> DeleteMessage(EmailIdRequest request, string token);

        Task<bool> SendDraftMessage(EmailIdRequest request, string token);

        Task<bool> ReplyMessage(string msgId, string recipients, string comment, string token);

        Task<bool> ReplyMultipleMessages(EmailReplyRequest request, string msgId, string token);

        Task<bool> ReplyAllMessage(string msgId, string comment, string token);

        Task<bool> ReplyAllMultipleMessages(EmailReplyAllRequest request, string msgId, string token);

        Task<bool> ForwardMessage(string msgId, string recipients, string comment, string token);

        Task<bool> ForwardMultipleMessages(EmailForwardRequest request, string msgId, string token);

        Task<bool> SendMail(EmailRequest request, string Token);

    }
}
